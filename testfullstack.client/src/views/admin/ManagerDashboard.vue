<template>
  <div class="max-w-4xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-3xl font-bold text-center mb-8">Панель управления</h1>
    <div class="grid grid-cols-1 sm:grid-cols-3 gap-6">
      <router-link to="/admin/users"
                   class="block bg-blue-600 text-white text-center py-4 rounded-lg shadow-md hover:bg-blue-700">
        Список пользователей
      </router-link>
      <router-link to="/admin/items"
                   class="block bg-blue-600 text-white text-center py-4 rounded-lg shadow-md hover:bg-blue-700">
        Редактирование каталога
      </router-link>
      <router-link to="/admin/orders"
                   class="block bg-blue-600 text-white text-center py-4 rounded-lg shadow-md hover:bg-blue-700">
        Список заказов
      </router-link>
      <router-link to="/admin/customers"
                   class="block bg-blue-600 text-white text-center py-4 rounded-lg shadow-md hover:bg-blue-700">
        Список заказчиков
      </router-link>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from "vue";

  export default defineComponent({
    name: "ManagerDashboard",
  });
</script>

<style scoped>
  body {
    background-color: #f3f4f6;
  }
</style>
