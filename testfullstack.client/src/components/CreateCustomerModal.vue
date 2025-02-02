<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <header class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">Добавить данные</h2>
        <button @click="close" class="text-gray-500 hover:text-gray-700 text-2xl">&times;</button>
      </header>
      <main>
        <form @submit.prevent="submitCustomer">
          <label class="block mb-2 font-semibold">Название компании</label>
          <input v-model="createCustomer.name" type="text" class="w-full p-2 mb-4 border rounded-lg" required />

          <label class="block mb-2 font-semibold">Адрес</label>
          <input v-model="createCustomer.address" type="text" class="w-full p-2 mb-4 border rounded-lg" />

          <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 w-full">
            Сохранить
          </button>
        </form>
      </main>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from "vue";
  import axios from "axios";
  import { useUserStore } from "@/stores/user";
  import { useRouter } from "vue-router";

  export default defineComponent({
    name: "CreateCustomerModal",
    props: { isOpen: Boolean, userId: String },
    emits: ["close"],
    setup(props, { emit }) {
      const userStore = useUserStore();
      const router = useRouter();
      const createCustomer = ref({ name: "", address: "", userId: props.userId });

      const submitCustomer = async () => {
        console.log("Токен:", userStore.token);
        try {
          const response = await axios.post("https://localhost:7034/api/customers/", createCustomer.value, {
            headers: { Authorization: `Bearer ${userStore.token}` },
          });
          console.log(response.data);
          if (response.data) {
            userStore.setCustomerId(response.data);
            console.log("Закрытие модального окна...");
            emit("close");
            close();
            router.push("/");
          }
        } catch (error) {
          console.error("Ошибка!", error.response?.data || error.message);
          alert("Не удалось сохранить данные заказчика.");
        }
      };

      const close = () => {
        emit("close");
      };

      return { createCustomer, submitCustomer, close };
    },
  });
</script>
