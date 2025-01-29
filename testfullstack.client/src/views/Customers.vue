<template>
  <div class="p-6">
    <h2 class="text-2xl text-blue font-semibold mb-4">Customers</h2>
    <ul>
      <li v-for="customer in customers" :key="customer.id" class="">
        {{ customer.name }} ({{ customer.code }})
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
  import { defineComponent, onMounted, ref } from "vue";
  import axios from "axios";

  export default defineComponent({
    name: "CustomersPage",
    setup() {
      const customers = ref([]);

      onMounted(async () => {
        try {
          const response = await axios.get("https://localhost:7034/api/customers");
          customers.value = response.data;
        } catch (error) {
          console.error("Error fetching customers", error);
        }
      });

      return { customers };
    },
  });
</script>
<!--<template>
  <div class="text-4xl font-bold text-blue-500">
    Tailwind работает!
  </div>
</template>-->
