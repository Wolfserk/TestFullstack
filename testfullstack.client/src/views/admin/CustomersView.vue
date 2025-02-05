<template>
  <div class="max-w-6xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-3xl font-bold text-center mb-6">Список заказчиков</h1>

    <div v-if="loading" class="text-center text-gray-500">Загрузка...</div>
    <div v-else-if="customers.length === 0" class="text-center text-gray-500">Нет заказчиков</div>

    <table v-else class="w-full border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">Название</th>
          <th class="border border-gray-300 px-4 py-2">Код</th>
          <th class="border border-gray-300 px-4 py-2">Адрес</th>
          <th class="border border-gray-300 px-4 py-2">Скидка</th>
          <th class="border border-gray-300 px-4 py-2">Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="customer in customers" :key="customer.id">
          <td class="border border-gray-300 px-4 py-2">{{ customer.name }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ customer.code }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ customer.address }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ customer.discount }}%</td>
          <td class="border border-gray-300 px-4 py-2 text-center">
            <button @click="editCustomer(customer)" class="bg-blue-600 text-white px-2 py-1 rounded-lg hover:bg-blue-700">
              ✎
            </button>
          </td>
        </tr>
      </tbody>
    </table>

   
    <CustomerModal v-if="isModalOpen"
                   :isOpen="isModalOpen"
                   :customer="selectedCustomer"
                   @close="isModalOpen = false"
                   @save="updateCustomer" />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from "vue";
  import axios from "axios";
  import { useUserStore } from "@/stores/user";
  import CustomerModal from "@/components/CustomerModal.vue";

  export default defineComponent({
    name: "CustomersView",
    components: { CustomerModal },
    setup() {
      const userStore = useUserStore();
      const customers = ref([]);
      const loading = ref(true);
      const isModalOpen = ref(false);
      const selectedCustomer = ref(null);

      const fetchCustomers = async () => {
        try {
          const response = await axios.get("https://localhost:7034/api/customers", {
            headers: { Authorization: `Bearer ${userStore.token}` }
          });
          customers.value = response.data;
        } catch (error) {
          console.error("Ошибка при загрузке заказчиков:");
        } finally {
          loading.value = false;
        }
      };

      const editCustomer = (customer: any) => {
        selectedCustomer.value = { ...customer };
        isModalOpen.value = true;
      };

      const updateCustomer = (updatedCustomer: any) => {
        const index = customers.value.findIndex(c => c.id === updatedCustomer.id);
        if (index !== -1) {
          customers.value[index] = updatedCustomer;
        }
      };

      onMounted(fetchCustomers);

      return { customers, loading, isModalOpen, selectedCustomer, editCustomer, updateCustomer };
    }
  });
</script>
