<template>
  <div class="flex flex-col lg:flex-row p-4">
  
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

    <div v-if="isConfirmModalOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white p-6 rounded-lg shadow-lg w-96">
        <h2 class="text-lg font-bold mb-4">Подтвердить заказ</h2>

        <label class="block mb-2">Введите номер заказа:</label>
        <input v-model="orderNumber" type="number" class="w-full p-2 border rounded-lg mb-4" />

        <label class="block mb-2">Выберите дату доставки:</label>
        <input v-model="shipmentDate"
               type="date"
               :min="minShipmentDate"
               class="w-full p-2 border rounded-lg mb-4" />

        <div class="flex justify-end">
          <button @click="confirmOrder" class="bg-green-500 text-white px-4 py-2 rounded mr-2">
            Подтвердить
          </button>
          <button @click="isConfirmModalOpen = false" class="bg-gray-500 text-white px-4 py-2 rounded">
            Отмена
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, computed } from "vue";
  import axios from "axios";
  import { useUserStore } from "@/stores/user";
  import type { Order } from "@/types";

  const userStore = useUserStore();
  const orders = ref<Order[]>([]);
  const selectedStatus = ref<string>("Все");
  const filteredOrdersList = ref<Order[]>([]);
  const isConfirmModalOpen = ref(false);
  const selectedOrderId = ref<string | null>(null);
  const orderNumber = ref("");
  const shipmentDate = ref("");

  
  const minShipmentDate = computed(() => {
    if (!selectedOrderId.value) return "";

    const selectedOrder = orders.value.find(order => order.id === selectedOrderId.value);
    if (!selectedOrder || !selectedOrder.orderDate) return "";

    return new Date(selectedOrder.orderDate).toISOString().split("T")[0];
  });

  const fetchOrders = async () => {
    try {
      const response = await axios.get<Order[]>("https://localhost:7034/api/orders", {
        headers: { Authorization: `Bearer ${userStore.token}` },
      });
      orders.value = response.data;
      applyFilters(); 
    } catch (error) {
      console.error("Ошибка при загрузке заказов:", error);
    }
  };

  const applyFilters = () => {
    const statusPriority = { 'Новый': 1, 'Выполняется': 2, 'Выполнен': 3 };

    filteredOrdersList.value = orders.value
      .filter(order => selectedStatus.value === 'Все' || order.status === selectedStatus.value)
      .sort((a, b) => statusPriority[a.status] - statusPriority[b.status]);
  };

  const formatDate = (date: string) => new Date(date).toLocaleDateString();

  const openConfirmModal = (orderId: string) => {
    selectedOrderId.value = orderId;
    orderNumber.value = "";
    shipmentDate.value = "";
    isConfirmModalOpen.value = true;
  };

  const confirmOrder = async () => {
    if (!selectedOrderId.value || !orderNumber.value || !shipmentDate.value) {
      alert("Заполните все поля!");
      return;
    }

    const selectedOrder = orders.value.find(order => order.id === selectedOrderId.value);
    if (selectedOrder && selectedOrder.orderDate) {
      const orderDate = new Date(selectedOrder.orderDate).getTime();
      const shipmentDateValue = new Date(shipmentDate.value).getTime();

      if (shipmentDateValue < orderDate) {
        alert("Дата доставки не может быть раньше даты заказа!");
        return;
      }
    }

    try {
      await axios.post(
        `https://localhost:7034/api/orders/confirm`,
        {
          orderId: selectedOrderId.value,
          orderNumber: parseInt(orderNumber.value),
          shipmentDate: shipmentDate.value,
        },
        { headers: { Authorization: `Bearer ${userStore.token}` } }
      );

      alert("Заказ подтвержден!");
      isConfirmModalOpen.value = false;
      await fetchOrders();
    } catch (error) {
      alert("Ошибка при подтверждении заказа.");
      console.error(error);
    }
  };

  const completeOrder = async (orderId: string) => {
    if (!confirm("Вы уверены, что хотите завершить заказ?")) return;

    try {
      await axios.post(
        `https://localhost:7034/api/orders/complete/${orderId}`,
        {},
        { headers: { Authorization: `Bearer ${userStore.token}` } }
      );

      alert("Заказ завершен!");
      await fetchOrders();
    } catch (error) {
      alert("Ошибка при завершении заказа.");
      console.error(error);
    }
  };

  onMounted(fetchOrders);
</script>
