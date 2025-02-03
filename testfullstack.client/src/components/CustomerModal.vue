<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <header class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">Редактировать заказчика</h2>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700 text-2xl">&times;</button>
      </header>
      <main>
        <form @submit.prevent="submitCustomer">
        
          <label class="block mb-2 font-semibold">Название компании</label>
          <input v-model="editedCustomer.name" type="text" class="w-full p-2 mb-4 border rounded-lg" required />

        
          <label class="block mb-2 font-semibold">Код</label>
          <input v-model="editedCustomer.code"
                 type="text"
                 pattern="^\d{4}-\d{4}$"
                 class="w-full p-2 mb-4 border rounded-lg"
                 required />
       
          <label class="block mb-2 font-semibold">Адрес</label>
          <input v-model="editedCustomer.address" type="text" class="w-full p-2 mb-4 border rounded-lg"/>

          
          <label class="block mb-2 font-semibold">Скидка (%)</label>
          <input v-model.number="editedCustomer.discount"
                 type="number"
                 min="0"
                 max="100"
                 class="w-full p-2 mb-4 border rounded-lg"
                 required />

          <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 w-full">
            Сохранить изменения
          </button>
        </form>
      </main>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, watch } from "vue";
  import axios from "axios";
  import { useUserStore } from "@/stores/user";

  export default defineComponent({
    name: "CustomerModal",
    props: {
      isOpen: { type: Boolean, required: true },
      customer: { type: Object, required: true }
    },
    emits: ["close", "save"],
    setup(props, { emit }) {
      const userStore = useUserStore();
      const editedCustomer = ref({ ...props.customer });

      watch(
        () => props.customer,
        (newCustomer) => {
          editedCustomer.value = { ...newCustomer };
        },
        { deep: true }
      );

      const submitCustomer = async () => {
        try {
          await axios.put(`https://localhost:7034/api/customers/`, editedCustomer.value, {
            headers: { Authorization: `Bearer ${userStore.token}` }
          });
          alert("Данные заказчика обновлены!");
          emit("save", editedCustomer.value);
          emit("close");
        } catch (error) {
          console.error("Ошибка при обновлении заказчика:", error.response?.data || error.message);
        }
      };

      return { editedCustomer, submitCustomer };
    }
  });
</script>
