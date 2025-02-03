import { createRouter, createWebHistory } from 'vue-router';

import Home from '../views/HomeView.vue';
import Register from '../views/RegisterView.vue';
import Orders from '../views/OrdersView.vue';
import Cart from '../views/CartView.vue';

import ManagerDashboard from "../views/admin/ManagerDashboard.vue";
import Users from "../views/admin/UsersView.vue";
import Items from "../views/admin/ItemsView.vue";
import AllOrders from "../views/admin/AllOrdersView.vue";
import Customers from "../views/admin/CustomersView.vue";

import { useUserStore } from "../stores/user"; 



const routes = [
  { path: '/', component: Home },
  { path: '/register', component: Register, meta: { requiresGuest: true } },
  { path: '/cart', component: Cart, meta: { requiresAuth: true, requiresCustomer: true } },
  { path: '/orders', component: Orders, meta: { requiresAuth: true, requiresCustomer: true } },


  { path: '/admin', component: ManagerDashboard, meta: { requiresManager: true } },
  { path: '/admin/users', component: Users, meta: { requiresManager: true } },
  { path: '/admin/items', component: Items, meta: { requiresManager: true } },
  { path: '/admin/orders', component: AllOrders, meta: { requiresManager: true } },
  { path: '/admin/customers', component: Customers, meta: { requiresManager: true } }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const userStore = useUserStore();
  const userRole = userStore.role || localStorage.getItem("userRole");
  const isAuthenticated = userStore.isAuthenticated;


  if (to.meta.requiresGuest) {
    if (isAuthenticated) {
      alert("Вы уже авторизованы. Регистрация недоступна.");
      next({ path: "/" }); 
    } else {
      next(); 
    }
    return;
  }

  //if (isAuthenticated && userRole === "Manager" && to.path !== "/admin") {
  //  next({ path: "/admin" }); // Перенаправляем на страницу администратора
  //  return;
  //}

  if (to.meta.requiresManager) {
    if (!isAuthenticated) {
      alert("Вы не авторизованы. Пожалуйста, войдите в систему.");
      next({ path: "/" });
    } else if (userRole !== "Manager") {
      alert("У вас нет доступа к этому разделу.");
      next("/");
    } else {
      next();
    }
    return;
  }

  if (to.meta.requiresCustomer) {
    if (!isAuthenticated) {
      alert("Вы не авторизованы. Пожалуйста, войдите в систему.");
      next({ path: "/" });
    } else if (userRole !== "Customer") {
      alert("Эта страница доступна только заказчикам.");
      next("/");
    } else {
      next();
    }
    return;
  }

  next();
});


export default router;
