<!--<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <header class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">Заполните данные</h2>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700">&times;</button>
      </header>
      <main>
        <form @submit.prevent="submitCustomer">
          <label class="block mb-2">Название компании</label>
          <input v-model="name" type="text" class="w-full p-2 mb-4 border rounded-lg" required />

          <label class="block mb-2">Адрес</label>
          <input v-model="address" type="text" class="w-full p-2 mb-4 border rounded-lg" required />

          <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 w-full">
            Сохранить
          </button>
        </form>
      </main>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, watch } from "vue";
  import axios from "axios";

  export default defineComponent({
    name: "CustomerModal",
    props: {
      isOpen: { type: Boolean, required: true },
      userId: { type: String, required: false, default: "" }
    },
    emits: ["close"],
    setup(props, { emit }) {
      const name = ref("");
      const address = ref("");

      watch(() => props.isOpen, (newVal) => {
        console.log("CustomerModal открыт:", newVal);
      });

      const submitCustomer = async () => {
        if (!props.userId) {
          console.error("Ошибка: userId отсутствует!");
          return;
        }

        try {
          await axios.post("https://localhost:7034/api/customers", {
            name: name.value,
            address: address.value,
            discount: 0,
            userId: props.userId,
          });

          alert("Данные сохранены!");
          emit("close");
        } catch (error) {
          console.error("Ошибка при сохранении данных:", error.response?.data || error.message);
        }
      };

      return { name, address, submitCustomer };
    },
  });
</script>-->

<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <header class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">Заполните данные</h2>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700">&times;</button>
      </header>
      <main>
        <form @submit.prevent="submitCustomer">
          <label class="block mb-2">Название компании</label>
          <input v-model="name" type="text" class="w-full p-2 mb-4 border rounded-lg" required />

          <label class="block mb-2">Адрес</label>
          <input v-model="address" type="text" class="w-full p-2 mb-4 border rounded-lg"/>

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
  import { useUserStore } from "../stores/user"; 

  export default defineComponent({
    name: "CustomerModal",
    props: {
      isOpen: { type: Boolean, required: true },
      userId: { type: String, required: true },
    },
    emits: ["close"],
    setup(props, { emit }) {
      const name = ref("");
      const address = ref("");
      const userStore = useUserStore(); 

      const submitCustomer = async () => {
        try {
          const response = await axios.post(
            "https://localhost:7034/api/customers",
            {
              name: name.value,
              address: address.value || null,
              userId: props.userId,
            },
            {
              headers: { Authorization: `Bearer ${userStore.token}` }, // Добавляем токен в заголовок
            }
          );

          if (response.status === 200) {
            alert("Данные успешно сохранены!");
            emit("close"); // Закрываем модальное окно
          }
        } catch (error) {
          console.error("Ошибка при сохранении данных:", error.response?.data || error.message);
          alert("Произошла ошибка при сохранении данных.");
        }
      };

      return { name, address, submitCustomer };
    },
  });
</script>
