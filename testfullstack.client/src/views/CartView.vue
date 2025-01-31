<template>
  <div class="max-w-4xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-3xl font-bold text-center mb-6">Корзина</h1>

    <div v-if="cart.length === 0" class="text-center text-gray-500">Корзина пуста</div>

    <table v-else class="w-full border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-4 py-2">Товар</th>
          <th class="border border-gray-300 px-4 py-2">Цена</th>
          <th class="border border-gray-300 px-4 py-2">Количество</th>
          <th class="border border-gray-300 px-4 py-2">Удалить</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in cart" :key="item.id">
          <td class="border border-gray-300 px-4 py-2">{{ item.name }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ item.price }} ₽</td>
          <td class="border border-gray-300 px-4 py-2">{{ item.quantity }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">
            <button @click="removeFromCart(item.id)"
                    class="bg-red-600 text-white px-2 py-1 rounded-lg hover:bg-red-700">
              ✕
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="mt-6 flex justify-between items-center">
      <p class="text-lg font-bold">Общая сумма: {{ total }} ₽</p>
      <button @click="submitOrder"
              class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700">
        Оформить заказ
      </button>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, computed, ref } from "vue";
  import { useCartStore } from "../stores/cart";

  export default defineComponent({
    name: "CartView",
    setup() {
      const cartStore = useCartStore();

      const cart = computed(() => cartStore.cart);

      const total = computed(() =>
        cart.value.reduce((sum, item) => sum + item.price * item.quantity, 0)
      );

      const submitOrder = async () => {
        await cartStore.submitOrder();
      };

      const removeFromCart = (id: string) => {
        cartStore.removeFromCart(id);
        cart.value = [...cartStore.cart];
      };

      return { cart, total, removeFromCart, submitOrder };
    },
  });
</script>
