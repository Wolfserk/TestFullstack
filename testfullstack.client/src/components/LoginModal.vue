<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <header class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">Вход</h2>
        <button @click="close" class="text-gray-500 hover:text-gray-700">&times;</button>
      </header>
      <main>
        <form @submit.prevent="login">
          <label class="block mb-2">Email</label>
          <input v-model="email" type="email" class="w-full p-2 mb-4 border border-gray-300 rounded-lg" required />

          <label class="block mb-2">Пароль</label>
          <input v-model="password" type="password" class="w-full p-2 mb-4 border border-gray-300 rounded-lg" required />

          <p v-if="errorMessage" class="text-red-500 text-sm mb-4">{{ errorMessage }}</p>

          <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 w-full">
            Войти
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
  import { useCartStore } from "../stores/cart";
  import { useRouter } from "vue-router";

  export default defineComponent({
    name: "LoginModal",
    props: { isOpen: Boolean },
    emits: ["close", "openCustomerModal"],
    setup(props, { emit }) {
      const email = ref("");
      const password = ref("");
      const errorMessage = ref("");
      const userStore = useUserStore();
      const cartStore = useCartStore();
      const router = useRouter();

      const login = async () => {
        errorMessage.value = "";
        try {
          const response = await axios.post("https://localhost:7034/api/auth/login", {
            email: email.value,
            password: password.value,
          });

          const { token, role, userId: id, customerId } = response.data;
          userStore.setUser(id, email.value, role, token, customerId);

          if (role === "Manager") {
            router.push("/admin");
          } else if (!customerId) {
            emit("openCustomerModal", id);
          } else {
            cartStore.loadCart();
            router.push("/");
          }
          emit("close");
        } catch (error) {
          errorMessage.value = "Неверный email или пароль.";
        }
      };



      const close = () => {
        emit("close");
      };

      return {
        email,
        password,
        errorMessage,
        login,
        close,
      };
    },
  });
</script>

