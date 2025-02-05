<template>
  <div id="app" class="min-h-screen flex flex-col bg-gray-100">
    <header class="bg-white shadow">
      <div class="max-w-7xl mx-auto py-4 px-4 sm:px-6 lg:px-8 flex justify-between items-center">
        <h1 class="text-xl font-bold text-gray-900">Test Application</h1>
        <nav>
          <ul v-if="isCustomer" class="flex space-x-4">
            <li><router-link to="/" class="text-blue-600 hover:underline">Главная</router-link></li>
            <li>
              <button @click="goToCart" class="relative text-blue-600 hover:underline">
                🛒 Корзина
                <span v-if="cartTotal > 0" class="ml-1 px-2 py-1 text-sm bg-red-500 rounded-full">
                  {{ cartTotal }}
                </span>
              </button>
            </li>
            <li>
              <router-link to="/orders" class="text-blue-600 hover:underline">Мои заказы</router-link>
            </li>
          </ul>
          <ul v-if="isManager" class="flex space-x-4">
            <li><router-link to="/admin" class="text-blue-600 hover:underline">Управление</router-link></li>
            <li><router-link to="/admin/users" class="text-blue-600 hover:underline">Пользователи</router-link></li>
            <li><router-link to="/admin/items" class="text-blue-600 hover:underline">Каталог</router-link></li>
            <li><router-link to="/admin/orders" class="text-blue-600 hover:underline">Заказы</router-link></li>
            <li><router-link to="/admin/customers" class="text-blue-600 hover:underline">Заказчики</router-link></li>
          </ul>
        </nav>
        <div>
          <div v-if="userStore.isAuthenticated" class="flex flex-col items-center space-y-2">
            <div>Добро пожаловать: <strong>{{ userStore.email }}</strong></div>
            <div>
              <strong>
                {{ userStore.role === 'Customer' ? `Заказчик` : "Менеджер" }}
              </strong>
            </div>
            <button @click="logout" class="bg-red-600 text-white py-2 px-4 rounded-lg hover:bg-red-700 w-fit mt-4">
              Выйти
            </button>
          </div>
          <div v-else>
            <button @click="openLoginModal" class="bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700">
              Войти
            </button>
            <router-link to="/register" class="ml-4 bg-gray-600 text-white py-2 px-4 rounded-lg hover:bg-gray-700">
              Регистрация
            </router-link>
          </div>
        </div>
      </div>
    </header>
    <main class="flex-grow">
      <router-view />
    </main>
    <footer class="bg-gray-200 text-center py-4">
      <p class="text-sm text-gray-600">&copy; 2025 Test App. Fullstack </p>
    </footer>

    <LoginModal v-if="isLoginModalOpen" :isOpen="isLoginModalOpen" @close="closeLoginModal" @openCustomerModal="openCustomerModal" />
    <CreateCustomerModal v-if="isCustomerModalOpen" :isOpen="isCustomerModalOpen" :userId="userId" @close="closeCustomerModal" />
  </div>
</template>

<script lang="ts">
  import { defineComponent, computed, ref, onMounted, watch } from "vue";
  import { useUserStore } from "./stores/user";
  import { useCartStore } from "./stores/cart";
  import { useRouter } from "vue-router";
  import LoginModal from "./components/LoginModal.vue";
  import CreateCustomerModal from "./components/CreateCustomerModal.vue";

  export default defineComponent({
    name: "App",
    components: { LoginModal, CreateCustomerModal },
    setup() {
      const userStore = useUserStore();
      const cartStore = useCartStore();
      const router = useRouter();

      const isLoginModalOpen = ref(false);
      const isCustomerModalOpen = ref(false);
      const userId = ref<string | null>(null);

      const goToCart = async () => {
        if (userStore.customerId === null || !userStore.customerId || userStore.customerId === "null") {
          isCustomerModalOpen.value = true;
          return;
        }
        else { router.push("/cart"); }
      };

      watch(() => userStore.customerId, (newCustomerId) => {
        if (newCustomerId) {
          isCustomerModalOpen.value = false;
        }
      });

      onMounted(() => {
        if (userStore.isAuthenticated && !userStore.customerId) {
          isCustomerModalOpen.value = true;
        }
      });

      const openLoginModal = () => {
        isLoginModalOpen.value = true;
      };

      const closeLoginModal = () => {
        isLoginModalOpen.value = false;
      };

      const openCustomerModal = (id: string) => {
        userId.value = id;
        isCustomerModalOpen.value = true;
      };

      const closeCustomerModal = () => {
        isCustomerModalOpen.value = false;
      };

      const isCustomer = computed(() => userStore.role === "Customer");
      const isManager = computed(() => userStore.role === "Manager");

      const cartTotal = computed(() =>
        cartStore.cart.reduce((sum, item) => sum + item.quantity, 0)
      );

      const logout = () => {
        userStore.clearUser();
        router.push("/");
      };

      return {
        isLoginModalOpen,
        openLoginModal,
        closeLoginModal,
        userStore,
        logout,
        isCustomer,
        cartTotal,
        isManager,
        isCustomerModalOpen,
        openCustomerModal,
        closeCustomerModal,
        userId,
        goToCart,
      };
    },
  });
</script>
