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
<div v-if="errorMessages.length" class="mt-4 text-red-500">
      <ul>
        <li v-for="(message, index) in errorMessages" :key="index">
          • {{ message }}
        </li>
      </ul>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from "vue";
  import axios from "axios";
  import { useRouter } from "vue-router";
  import { useUserStore } from "../stores/user";

  export default defineComponent({
    name: "RegisterView",
    setup() {
      const email = ref("");
      const password = ref("");
      const confirmPassword = ref("");
      const errorMessages = ref<string[]>([]);
      const router = useRouter();
      const userStore = useUserStore();
      const userId = ref<string | null>(null);

      const errorTranslations: Record<string, string> = {
        "Passwords must be at least 6 characters.": "Пароль должен содержать не менее 6 символов.",
        "Passwords must have at least one non alphanumeric character.": "Пароль должен содержать хотя бы один специальный символ.",
        "Passwords must have at least one lowercase ('a'-'z').": "Пароль должен содержать хотя бы одну строчную букву.",
        "Passwords must have at least one uppercase ('A'-'Z').": "Пароль должен содержать хотя бы одну заглавную букву.",
        "Passwords must have at least one digit ('0'-'9').": "Пароль должен содержать хотя бы одну цифру.",
        "Username is already taken.": "Email уже занят.",
      };

      const register = async () => {
        errorMessages.value = [];

        if (password.value !== confirmPassword.value) {
          errorMessages.value.push("Пароли не совпадают!");
          return;
        }

        try {
          console.log("🔹 Отправляем запрос на регистрацию...");
          const response = await axios.post("https://localhost:7034/api/auth/register", {
            email: email.value,
            password: password.value,
            confirmPassword: confirmPassword.value,
          });

          console.log("✅ Регистрация успешна:", response.data);

          // 🔹 Вход после регистрации
          const loginResponse = await axios.post("https://localhost:7034/api/auth/login", {
            email: email.value,
            password: password.value,
          });

          console.log("✅ Вход выполнен:", loginResponse.data);

          const { token, role, userId: id, customerId } = loginResponse.data;
          userStore.setUser(id, email.value, role, token, null );
          userId.value = id;
          if (userStore.role == 'Manager')
          router.push("/admin");
          else
          router.push("/");
          
        } catch (error: any) {
          console.error("❌ Ошибка при регистрации:", error.response?.data || error.message);

          if (Array.isArray(error.response?.data)) {
            errorMessages.value = error.response.data.map((err: { description: string }) =>
              errorTranslations[err.description] || "Ошибка: " + err.description
            );
          } else {
            errorMessages.value.push(error.response?.data || "Ошибка регистрации.");
          }
        }
      };

      return { email, password, confirmPassword, errorMessages, register, userId };
    },
  });
</script>

