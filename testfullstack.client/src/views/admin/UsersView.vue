<template>
  <div class="max-w-4xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-2xl font-bold text-center mb-6">Пользователи</h1>
    <button @click="openAddModal"
            class="mb-4 bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700">
      Добавить пользователя
    </button>
    <table class="w-full border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">Email</th>
          <th class="border border-gray-300 px-4 py-2">Роль</th>
          <th class="border border-gray-300 px-4 py-2">Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td class="border border-gray-300 px-4 py-2">{{ user.email }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">{{ user.role }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">
            <button @click="openEditModal(user)"
                    class="bg-yellow-500 text-white py-1 px-3 rounded-lg hover:bg-yellow-600 mr-2">
              Редактировать
            </button>
            <button @click="deleteUser(user.id)"
                    class="bg-red-600 text-white py-1 px-3 rounded-lg hover:bg-red-700">
              Удалить
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <UsersModal v-if="isAddModalOpen"
                :isOpen="isAddModalOpen"
                title="Добавить пользователя"
                @close="closeAddModal"
                @submit="submitAddModal">
      <form>
        <label class="block mb-2">Email</label>
        <input type="email"
               v-model="newUser.email"
               class="w-full p-2 mb-4 border border-gray-300 rounded-lg" />
        <label class="block mb-2">Роль</label>
        <select v-model="newUser.role"
                class="w-full p-2 mb-4 border border-gray-300 rounded-lg">
          <option value="Customer">Заказчик</option>
          <option value="Manager">Менеджер</option>
        </select>
        <label class="block mb-2">Пароль</label>
        <input type="password"
               v-model="newUser.password"
               class="w-full p-2 mb-4 border border-gray-300 rounded-lg" />
      </form>
    </UsersModal>


    <UsersModal v-if="isEditModalOpen"
                :isOpen="isEditModalOpen"
                title="Редактировать пользователя"
                @close="closeEditModal"
                @submit="submitEditModal">
      <form>
        <label class="block mb-2">Email</label>
        <input type="email"
               v-model="editUser.email"
               class="w-full p-2 mb-4 border border-gray-300 rounded-lg"
                />
        <label class="block mb-2">Роль</label>
        <select v-model="editUser.role"
                class="w-full p-2 mb-4 border border-gray-300 rounded-lg">
          <option value="Customer">Заказчик</option>
          <option value="Manager">Менеджер</option>
        </select>
      </form>
    </UsersModal>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from "vue";
  import UsersModal from "../../components/UsersModal.vue";
  import axios from "axios";
  import { useUserStore } from "../../stores/user";

  export default defineComponent({
    name: "UsersView",
    components: { UsersModal },
    setup() {
      const userStore = useUserStore();
      const users = ref([]);
      const isAddModalOpen = ref(false);
      const isEditModalOpen = ref(false);
      const newUser = ref({ email: "", password: "", confirmPassword: "", role: "Customer" });
      const editUser = ref({ id: "", email: "", role: "" });

      const isManager = ref(userStore.role === "Manager");

      const fetchUsers = async () => {
        if (!isManager.value) return;
        try {
          const response = await axios.get("https://localhost:7034/api/users", {
            headers: { Authorization: `Bearer ${userStore.token}` },
          });
          
          //users.value = response.data;
          users.value = response.data.filter((user: any) => user.id !== userStore.userId);
        } catch (error) {
          console.error("Ошибка при загрузке пользователей:", error);
        }
      };

      const openAddModal = () => (isAddModalOpen.value = true);
      const closeAddModal = () => (isAddModalOpen.value = false);
      const submitAddModal = async () => {
        try {
          newUser.value.confirmPassword = newUser.value.password
          console.log(newUser.value);
          await axios.post("https://localhost:7034/api/users/add", newUser.value, {
            headers: { Authorization: `Bearer ${userStore.token}` },
          });   
          alert("Пользователь добавлен!");
          fetchUsers();
          closeAddModal();
        } catch (error) {
          alert("Ошибка при добавлении, проверьте корректность введенных данных!");
          console.error("Ошибка при добавлении пользователя:", error);
        }
      };

      const openEditModal = (user: any) => {
        editUser.value = { ...user };
        isEditModalOpen.value = true;
      };
      const closeEditModal = () => (isEditModalOpen.value = false);
      const submitEditModal = async () => {
        try {
          await axios.put(`https://localhost:7034/api/users/`, editUser.value, {
            headers: { Authorization: `Bearer ${userStore.token}` }
          });
          alert("Информация о пользователе обновлена!");
          fetchUsers();
          closeEditModal();
        } catch (error) {
          alert("Ошибка при редактировании, проверьте корректность введенных данных!");
          console.error("Ошибка при редактировании пользователя:", error);
        }
      };

      const deleteUser = async (userId: string) => {
        if (confirm("Вы уверены, что хотите удалить пользователя?")) {
          try {
            await axios.delete(`https://localhost:7034/api/users/${userId}`, {
              headers: { Authorization: `Bearer ${userStore.token}` },
            });
            alert("Пользователь успешно удален.");
            fetchUsers();
          } catch (error) {
            alert("Ошибка при удалении пользователя!");
            console.error("Ошибка при удалении пользователя:", error);
          }
        }
      };

      onMounted(fetchUsers);

      return {
        users,
        isAddModalOpen,
        isEditModalOpen,
        newUser,
        editUser,
        isManager,
        openAddModal,
        closeAddModal,
        submitAddModal,
        openEditModal,
        closeEditModal,
        submitEditModal,
        deleteUser,
      };
    },
  });
</script>


