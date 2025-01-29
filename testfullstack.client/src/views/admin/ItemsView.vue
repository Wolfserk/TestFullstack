<template>
  <div class="max-w-4xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-2xl font-bold text-center mb-6">Товары</h1>
    <button @click="openAddModal" class="mb-4 bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700">
      Добавить товар
    </button>
    <table class="w-full border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">Название</th>
          <th class="border border-gray-300 px-4 py-2">Цена</th>
          <th class="border border-gray-300 px-4 py-2">Категория</th>
          <th class="border border-gray-300 px-4 py-2">Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.id">
          <td class="border border-gray-300 px-4 py-2">{{ item.name }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ item.price }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ item.category }}</td>
          <td class="border border-gray-300 px-4 py-2">
            <button @click="openEditModal(item)" class="bg-yellow-500 text-white py-1 px-3 rounded-lg hover:bg-yellow-600 mr-2">
              Редактировать
            </button>
            <button @click="deleteItem(item.id)" class="bg-red-600 text-white py-1 px-3 rounded-lg hover:bg-red-700">
              Удалить
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Модальное окно для добавления и редактирования -->
    <ItemModal v-if="isModalOpen" :isOpen="isModalOpen" :title="modalTitle" :itemData="currentItem"
               @close="closeModal" @submit="submitModal" />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from "vue";
  import axios from "axios";
  import ItemModal from "../../components/ItemModal.vue";
  import { useUserStore } from "../../stores/user";

  export default defineComponent({
    name: "ItemsView",
    components: { ItemModal },
    setup() {
      const userStore = useUserStore();
      const items = ref([]);
      const isModalOpen = ref(false);
      const modalTitle = ref("");
      const currentItem = ref({ id: null, name: "", price: 0, category: "" });

      const fetchItems = async () => {
        try {
          const response = await axios.get("https://localhost:7034/api/items");
          items.value = response.data;
        } catch (error) {
          console.error("Ошибка при загрузке товаров:", error);
        }
      };

      const openAddModal = () => {
        modalTitle.value = "Добавить товар";
        currentItem.value = { id: null, name: "", price: 0, category: "" };
        isModalOpen.value = true;
      };

      const openEditModal = (item: any) => {
        modalTitle.value = "Редактировать товар";
        currentItem.value = { ...item };
        isModalOpen.value = true;
      };

      const closeModal = () => {
        isModalOpen.value = false;
        currentItem.value = { id: null, name: "", price: 0, category: "" };
      };

      const submitModal = async (itemData: any) => {
        try {
          if (itemData.id) {
            await axios.put(`https://localhost:7034/api/items/${itemData.id}`, itemData, {
              headers: { Authorization: `Bearer ${userStore.token}` },
            });
          } else {
            await axios.post("https://localhost:7034/api/items", itemData, {
              headers: { Authorization: `Bearer ${userStore.token}` },
            });
          }
          fetchItems();
          closeModal();
        } catch (error) {
          console.error("Ошибка при сохранении товара:", error);
        }
      };

      const deleteItem = async (id: string) => {
        if (confirm("Вы уверены, что хотите удалить товар?")) {
          try {
            await axios.delete(`https://localhost:7034/api/items/${id}`, {
              headers: { Authorization: `Bearer ${userStore.token}` },
            });
            fetchItems();
          } catch (error) {
            console.error("Ошибка при удалении товара:", error);
          }
        }
      };

      onMounted(fetchItems);

      return {
        items,
        isModalOpen,
        modalTitle,
        currentItem,
        openAddModal,
        openEditModal,
        closeModal,
        submitModal,
        deleteItem,
      };
    },
  });
</script>
