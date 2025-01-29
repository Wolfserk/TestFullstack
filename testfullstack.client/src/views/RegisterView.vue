<template>
  <div class="max-w-md mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-2xl font-bold mb-4 text-center">Регистрация</h1>
    <form @submit.prevent="register">
      <div class="mb-4">
        <label class="block text-gray-700">Email</label>
        <input v-model="email"
               type="email"
               required
               class="w-full px-4 py-2 border rounded-lg" />
      </div>
      <div class="mb-4">
        <label class="block text-gray-700">Пароль</label>
        <input v-model="password"
               type="password"
               required
               class="w-full px-4 py-2 border rounded-lg" />
      </div>
      <div class="mb-4">
        <label class="block text-gray-700">Подтверждение пароля</label>
        <input v-model="confirmPassword"
               type="password"
               required
               class="w-full px-4 py-2 border rounded-lg" />
      </div>
      <button type="submit"
              class="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700">
        Зарегистрироваться
      </button>
    </form>
    <p v-if="errorMessage" class="text-red-500 mt-4">{{ errorMessage }}</p>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from "vue";
  import axios from "axios";
  import { useRouter } from "vue-router";

  export default defineComponent({
    name: "RegisterPage",
    setup() {
      const email = ref("");
      const password = ref("");
      const confirmPassword = ref("");
      const errorMessage = ref("");
      const router = useRouter();

      const register = async () => {
        if (password.value !== confirmPassword.value) {
          errorMessage.value = "Пароли не совпадают!";
          return;
        }

        try {
          await axios.post("https://localhost:7034/api/auth/register", {
            email: email.value,
            password: password.value,
            confirmPassword: confirmPassword.value,
          });
         /* alert("Регистрация прошла успешно!");*/
          router.push({ path: "/", query: { loginModal: "true" } }); // Перенаправление на главную с параметром
        } catch (error: any) {
          errorMessage.value = error.response?.data || "Ошибка регистрации.";
        }
      };

      return { email, password, confirmPassword, errorMessage, register };
    },
  });
</script>



<style scoped>
  body {
    background-color: #f3f4f6;
  }
</style>
