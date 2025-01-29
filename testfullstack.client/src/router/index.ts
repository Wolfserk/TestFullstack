import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/HomeView.vue';
import Customers from '../views/Customers.vue';
import Register from '../views/RegisterView.vue';
import Cart from '../views/CartView.vue';
import ManagerDashboard from "../views/admin/ManagerDashboard.vue";
import Users from "../views/admin/UsersView.vue";
import Items from "../views/admin/ItemsView.vue";
import { useUserStore } from "../stores/user"; 



const routes = [
  { path: '/', component: Home },
  { path: '/customers', component: Customers },
  { path: '/register', component: Register },
  { path: '/cart', component: Cart },
  { path: '/admin', component: ManagerDashboard, meta: { requiresManager: true } },
  { path: '/admin/users', component: Users, meta: { requiresManager: true } },
  { path: '/admin/items', component: Items, meta: { requiresManager: true } }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Middleware для проверки роли
router.beforeEach((to, from, next) => {
  const userStore = useUserStore();
  const isManagerRoute = to.path.startsWith("/admin");
  const isAuthenticated = userStore.isAuthenticated;
  const isManager = userStore.role === "Manager";
  const userRole = userStore.role || localStorage.getItem("userRole");
  //const isCustomer = userStore.role === "Customer";

  if (to.meta.requiresManager && userRole !== "Manager") {
    if (!isAuthenticated) {
      alert("Вы не авторизованы. Пожалуйста, войдите в систему.");
      next({ path: "/" });
    } else
    if (isManagerRoute && userStore.role !== "Manager") {
      alert("У вас нет доступа к этому разделу.");
      next("/");
    } 
    else if (!isManager) {
      alert("У вас нет доступа к этой странице.");
      next(false);
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;
