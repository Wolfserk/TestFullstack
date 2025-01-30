<!--<template>
  <div id="app" class="min-h-screen flex flex-col bg-gray-100">
    <header class="bg-white shadow">
      <div class="max-w-7xl mx-auto py-4 px-4 sm:px-6 lg:px-8 flex justify-between items-center">
        <h1 class="text-xl font-bold text-gray-900">Test Application</h1>
        <nav>
          <ul class="flex space-x-4">
            <li><router-link to="/" class="text-blue-600 hover:underline">Главная</router-link></li>
            <li>
              <div v-if="isCustomer" class="flex items-center space-x-4">
                <router-link to="/cart" class="relative text-blue-600 hover:underline">
                  🛒 Корзина
                  <span v-if="cartTotal > 0" class="ml-1 px-2 py-1 text-sm bg-red-500 rounded-full">
                    {{ cartTotal }}
                  </span>
                </router-link>
              </div>
            </li>
            <li><router-link v-if="isManager" to="/admin" class="text-blue-600 hover:underline">Управление</router-link></li>
          </ul>
        </nav>
        <div>
          <div v-if="userStore.isAuthenticated" class="space-x-4">
            Добро пожаловать: <strong>{{ userStore.email }}</strong>
            <button @click="logout" class="bg-red-600 text-white py-2 px-4 rounded-lg hover:bg-red-700">
              Выйти
            </button>
          </div>
          <div v-else>
            <button @click="openLoginModal"
                    class="bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700">
              Войти
            </button>
            <router-link to="/register"
                         class="ml-4 bg-gray-600 text-white py-2 px-4 rounded-lg hover:bg-gray-700">
              Регистрация
            </router-link>
          </div>
          <LoginModal v-if="isLoginModalOpen" :isOpen="isLoginModalOpen" :onClose="closeLoginModal" />

          <CustomerModal v-if="isCustomerModalOpen"
                         :isOpen="isCustomerModalOpen"
                         :userId="userId"
                         @close="isCustomerModalOpen = false" />
        </div>
      </div>
    </header>
    <main class="flex-grow">
      <router-view />
    </main>
    <footer class="bg-gray-200 text-center py-4">
      <p class="text-sm text-gray-600">&copy; 2025 Test Application</p>
    </footer>
  </div>
</template>


<script lang="ts">
  import { defineComponent, computed, ref } from "vue";
  import { useUserStore } from "./stores/user";
  import LoginModal from "./components/LoginModal.vue";
  import CustomerModal from "./components/CustomerModal.vue";
  import { useCartStore } from "./stores/cart";
  import { useRoute, useRouter } from "vue-router";
  import { storeToRefs } from "pinia";

 

  export default defineComponent({
    name: "App",
    components: {LoginModal, CustomerModal},
    setup() {
      const userStore = useUserStore();
      const cartStore = useCartStore();

      const route = useRoute();
      const router = useRouter();
      const isCustomerModalOpen = ref(false);

      const isLoginModalOpen = ref(false);

      const openLoginModal = () => {
        isLoginModalOpen.value = true;
      };
      const closeLoginModal = () => {
        console.log(isCustomerModalOpen.value);
        isLoginModalOpen.value = false;
      };

      const isCustomer = computed(() => userStore.role === "Customer");
      const isManager = computed(() => userStore.role === "Manager");


      const cartTotal = computed(() =>
        cartStore.cart.reduce((sum, item) => sum + item.quantity, 0)
      );

      const logout = () => {
        userStore.clearUser();
        router.push('/');
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
        isCustomerModalOpen
      };
    },
  });
</script>-->

<template>
  <div id="app" class="min-h-screen flex flex-col bg-gray-100">
    <header class="bg-white shadow">
      <div class="max-w-7xl mx-auto py-4 px-4 sm:px-6 lg:px-8 flex justify-between items-center">
        <h1 class="text-xl font-bold text-gray-900">Test Application</h1>
        <nav>
          <ul class="flex space-x-4">
            <li><router-link to="/" class="text-blue-600 hover:underline">Главная</router-link></li>
            <li>
              <div v-if="isCustomer" class="flex items-center space-x-4">
                <router-link to="/cart" class="relative text-blue-600 hover:underline">
                  🛒 Корзина
                  <span v-if="cartTotal > 0" class="ml-1 px-2 py-1 text-sm bg-red-500 rounded-full">
                    {{ cartTotal }}
                  </span>
                </router-link>
              </div>
            </li>
            <li><router-link v-if="isManager" to="/admin" class="text-blue-600 hover:underline">Управление</router-link></li>
          </ul>
        </nav>
        <div>
          <div v-if="userStore.isAuthenticated" class="space-x-4">
            Добро пожаловать: <strong>{{ userStore.email }}</strong>
            <button @click="logout" class="bg-red-600 text-white py-2 px-4 rounded-lg hover:bg-red-700">
              Выйти
            </button>
          </div>
          <div v-else>
            <button @click="openLoginModal"
                    class="bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700">
              Войти
            </button>
            <router-link to="/register"
                         class="ml-4 bg-gray-600 text-white py-2 px-4 rounded-lg hover:bg-gray-700">
              Регистрация
            </router-link>
          </div>
          <LoginModal v-if="isLoginModalOpen"
                      :isOpen="isLoginModalOpen"
                      @close="closeLoginModal"
                      @openCustomerModal="openCustomerModal" />

          <CustomerModal v-if="isCustomerModalOpen"
                         :isOpen="isCustomerModalOpen"
                         :userId="userId"
                         @close="closeCustomerModal" />
        </div>
      </div>
    </header>
    <main class="flex-grow">
      <router-view />
    </main>
    <footer class="bg-gray-200 text-center py-4">
      <p class="text-sm text-gray-600">&copy; 2025 Test App. Fullstack </p>
    </footer>
  </div>
</template>

<script lang="ts">
  import { defineComponent, computed, ref } from "vue";
  import { useUserStore } from "./stores/user";
  import { useCartStore } from "./stores/cart";
  import { useRouter } from "vue-router";
  import LoginModal from "./components/LoginModal.vue";
  import CustomerModal from "./components/CustomerModal.vue"; // Импортируйте CustomerModal

  export default defineComponent({
    name: "App",
    components: { LoginModal, CustomerModal }, // Добавьте CustomerModal в компоненты
    setup() {
      const userStore = useUserStore();
      const cartStore = useCartStore();
      const router = useRouter();

      const isLoginModalOpen = ref(false);
      const isCustomerModalOpen = ref(false); // Состояние для CustomerModal
      const userId = ref<string | null>(null); // userId для передачи в CustomerModal

      const openLoginModal = () => {
        isLoginModalOpen.value = true;
      };

      const closeLoginModal = () => {
        isLoginModalOpen.value = false;
      };

      const openCustomerModal = (id: string) => {
        userId.value = id; // Устанавливаем userId
        isCustomerModalOpen.value = true; // Открываем CustomerModal
      };

      const closeCustomerModal = () => {
        isCustomerModalOpen.value = false; // Закрываем CustomerModal
      };

      const isCustomer = computed(() => userStore.role === "Customer");
      const isManager = computed(() => userStore.role === "Manager");

      const cartTotal = computed(() =>
        cartStore.cart.reduce((sum, item) => sum + item.quantity, 0)
      );

      const logout = () => {
        userStore.clearUser();
        router.push('/');
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
      };
    },
  });
</script>
