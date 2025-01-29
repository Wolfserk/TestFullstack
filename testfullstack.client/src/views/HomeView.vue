<template>
  <div class="max-w-6xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-3xl font-bold text-center mb-6">Каталог товаров</h1>

    <div v-if="loading" class="text-center">Загрузка...</div>
    <div v-else-if="items.length === 0" class="text-center text-gray-500">Нет доступных товаров</div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="item in items" :key="item.id" class="border p-4 rounded-lg shadow">
        <h2 class="text-xl font-semibold">{{ item.name }}</h2>
        <p class="text-gray-700">Категория: {{ item.category }}</p>
        <p class="text-lg font-bold text-blue-600">{{ item.price }} ₽</p>
        <p class="text-sm text-gray-500">Код: {{ item.code }}</p>

        <div v-if="isCustomer" class="mt-2 flex items-center">
          <input v-model="quantities[item.id]" type="number" min="1" max="100"
                 class="w-16 p-1 border rounded-lg text-center" />
          <button @click="addToCart(item, quantities[item.id])"
                  class="ml-2 bg-green-600 text-white px-4 py-1 rounded-lg hover:bg-green-700">
            Добавить в корзину
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted, reactive, computed } from "vue";
  import axios from "axios";
  import { useCartStore } from "../stores/cart";
  import { useUserStore } from "../stores/user";

  export default defineComponent({
    name: "HomeView",
    setup() {
      const items = ref([]);
      const loading = ref(true);
      const cartStore = useCartStore();
      const quantities = reactive<{ [key: string]: number }>({});
      const userStore = useUserStore();
      const isCustomer = computed(() => userStore.role === "Customer");

      const fetchItems = async () => {
        try {
          const response = await axios.get("https://localhost:7034/api/items");
          items.value = response.data;
          response.data.forEach((item: any) => (quantities[item.id] = 1));
        } catch (error) {
          console.error("Ошибка при загрузке товаров:", error);
        } finally {
          loading.value = false;
        }
      };

      const addToCart = (item: any, quantity: number) => {
        cartStore.addToCart(item, quantity);
      };

      onMounted(fetchItems);

      return { items, loading, quantities, addToCart, isCustomer };
    },
  });
</script>
