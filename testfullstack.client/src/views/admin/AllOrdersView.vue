<template>
  <div class="flex flex-col lg:flex-row p-4">
    <!-- Панель фильтрации -->
    <div class="lg:w-1/6 lg:pr-4 mb-6 lg:mb-0">
      <h2 class="text-xl font-bold mb-4">Фильтры</h2>
      <label class="block mb-2">Статус заказа:</label>
      <select v-model="selectedStatus" class="w-full p-2 border rounded-lg mb-4">
        <option value="Все">Все</option>
        <option value="Новый">Новый</option>
        <option value="Выполняется">Выполняется</option>
        <option value="Выполнен">Выполнен</option>
      </select>
      <button @click="applyFilters" class="w-full bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700">
        Применить
      </button>
    </div>

    <!-- Таблица заказов -->
    <div class="flex-1 overflow-x-auto">
      <h1 class="text-2xl font-bold text-center mb-6">Список заказов</h1>
      <table class="min-w-full border-collapse border border-gray-300">
        <thead>
          <tr class="bg-gray-200">
            <th class="border p-3 text-lg text-center">№ заказа</th>
            <th class="border p-3 text-lg text-center">Дата</th>
            <th class="border p-3 text-lg text-center">Дата доставки</th>
            <th class="border p-3 text-lg text-center">Действия</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="order in filteredOrdersList" :key="order.id" class="border">
            <td class="p-3 text-lg text-center">{{ order.orderNumber ?? "—" }}</td>
            <td class="p-3 text-lg text-center">{{ formatDate(order.orderDate) }}</td>
            <td class="p-3 text-lg text-center">{{ order.shipmentDate ? formatDate(order.shipmentDate) : "—" }}</td>
            <td class="p-3 text-lg text-center">
              <button v-if="order.status === 'Новый'"
                      @click="openConfirmModal(order.id)"
                      class="px-4 py-2 bg-green-500 text-white rounded-lg">
                Подтвердить
              </button>
              <button v-if="order.status === 'Выполняется'"
                      @click="completeOrder(order.id)"
                      class="px-4 py-2 bg-blue-500 text-white rounded-lg">
                Завершить
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from "vue";
  import axios from "axios";
  import { useUserStore } from "@/stores/user";
  import type { Order } from "@/types";

  const userStore = useUserStore();
  const orders = ref<Order[]>([]);
  const selectedStatus = ref<string>("Все");
  const filteredOrdersList = ref<Order[]>([]);

  const fetchOrders = async () => {
    try {
      const response = await axios.get<Order[]>("https://localhost:7034/api/orders", {
        headers: { Authorization: `Bearer ${userStore.token}` },
      });
      orders.value = response.data;
      filteredOrdersList.value = [...orders.value]; 
    } catch (error) {
      console.error("Ошибка при загрузке заказов:", error);
    }
  };

  const applyFilters = () => {
    filteredOrdersList.value = orders.value
      .filter(order => selectedStatus.value === 'Все' || order.status === selectedStatus.value)
      .sort((a, b) => {
        const orderPriority = { 'Новый': 1, 'Выполняется': 2, 'Выполнен': 3 };
        return orderPriority[a.status] - orderPriority[b.status];
      });
  };

  const formatDate = (date: string) => new Date(date).toLocaleDateString();

  onMounted(fetchOrders);
</script>
