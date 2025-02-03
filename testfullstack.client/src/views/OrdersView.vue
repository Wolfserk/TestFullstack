<template>
  <div class="flex justify-center">
    <div class="w-1/6 p-4">
      <h2 class="text-lg font-bold mb-4">Фильтр</h2>

      <label class="block mb-2">Фильтр по статусу:</label>
      <select v-model="selectedStatusTemp"
              class="w-full p-2 border rounded-lg mb-4">
        <option value="">Все</option>
        <option value="Новый">Новый</option>
        <option value="Выполняется">Выполняется</option>
        <option value="Выполнен">Выполнен</option>
      </select>

      <button @click="applyFilter"
              class="w-full bg-blue-500 text-white px-4 py-2 rounded-lg">
        Применить
      </button>
    </div>

    <div class="w-2/5 p-4">

      <h1 class="text-2xl font-bold mb-4 text-center">Мои заказы</h1>

      <div v-if="loading" class="text-center">Загрузка...</div>
      <div v-else>
        <div v-for="order in filteredOrders" :key="order.id" class="bg-white shadow rounded-lg p-4 mb-4">
          <div class="flex justify-between items-center">
            <div>
              <h2 class="text-lg font-bold">
                {{ order.orderNumber !== null && order.orderNumber !== 0 ? `Заказ #${order.orderNumber}` : "Заказу скоро будет присвоен номер" }}
              </h2>
              <p class="text-sm text-gray-600">Дата: {{ formatDate(order.orderDate) }}</p>
              <p class="text-sm text-gray-600">Статус: {{ order.status }}</p>
            </div>
            <div>
              <button v-if="order.status === 'Новый'"
                      @click="cancelOrder(order.id)"
                      class="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600">
                Отменить
              </button>
            </div>
          </div>
          <div class="mt-4">
            <h3 class="font-bold">Товары:</h3>
            <ul>
              <li v-for="item in order.orderItems" :key="item.id" class="text-sm text-gray-600">
                {{ item.itemName }} - {{ item.itemsCount }} шт. по {{ item.itemPrice }} ₽
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, computed, onMounted } from "vue";
  import axios from "axios";
  import { useUserStore } from "../stores/user";

  export default defineComponent({
    name: "OrdersView",
    setup() {
      const userStore = useUserStore();
      const orders = ref([]);
      const loading = ref(true);
      const selectedStatus = ref("");
      const selectedStatusTemp = ref("");

      const applyFilter = () => {
        selectedStatus.value = selectedStatusTemp.value;
      };

      const fetchOrders = async () => {
        try {
          const response = await axios.get("https://localhost:7034/api/orders/myorders", {
            headers: { Authorization: `Bearer ${userStore.token}` },
          });
          orders.value = response.data;
        } catch (error) {
          console.error("Ошибка при загрузке заказов:", error);
        } finally {
          loading.value = false;
        }
      };

      const filteredOrders = computed(() => {
        if (!selectedStatus.value) return orders.value;
        return orders.value.filter((order) => order.status === selectedStatus.value);
      });

      const cancelOrder = async (orderId: string) => {
        if (confirm("Вы уверены, что хотите отменить заказ?")) {
          try {
            await axios.delete(`https://localhost:7034/api/orders/${orderId}`, {
              headers: { Authorization: `Bearer ${userStore.token}` },
            });
            await fetchOrders();
          } catch (error) {
            console.error("Ошибка при отмене заказа:", error);
            alert("Не удалось отменить заказ.");
          }
        }
      };

      const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleDateString("ru-RU", {
          year: "numeric",
          month: "long",
          day: "numeric",
          hour: "2-digit",
          minute: "2-digit",
        });
      };

      onMounted(() => {
        fetchOrders();
      });

      return {
        orders,
        loading,
        selectedStatus,
        selectedStatusTemp,
        filteredOrders,
        cancelOrder,
        formatDate,
        applyFilter,
      };
    },
  });
</script>
