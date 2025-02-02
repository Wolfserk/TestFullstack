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
          <td class="border border-gray-300 px-4 py-2">
            <span v-if="discount > 0" class="text-gray-500 line-through mr-2">{{ item.price }} ₽</span>
            <span class="text-blue-600">{{ getDiscountedPrice(item.price) }} ₽</span>
          </td>
          <td class="border border-gray-300 px-4 py-2">{{ item.quantity }}</td>
          <td class="border border-gray-300 px-4 py-2 text-center">
            <button @click="removeFromCart(item.id)" class="bg-red-600 text-white px-2 py-1 rounded-lg hover:bg-red-700">
              ✕
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="mt-6 flex justify-between items-center">
      <p class="text-lg font-bold">Общая сумма: {{ total }} ₽</p>
      <button @click="handleOrder" class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700">
        Оформить заказ
      </button>
    </div>
  </div>
</template>
<script lang="ts">
  import { defineComponent, computed, ref, onMounted, watch } from "vue";
  import { useCartStore } from "../stores/cart";
  import { useUserStore } from "../stores/user";
  import { useRouter } from "vue-router";
  import CreateCustomerModal from "../components/CreateCustomerModal.vue";

  export default defineComponent({
    name: "CartView",
    components: { CreateCustomerModal },
    setup() {
      const cartStore = useCartStore();
      const userStore = useUserStore();
      const router = useRouter();
      const isCustomerModalOpen = ref(false);

      // ✅ Проверяем `customerId` при загрузке страницы
      onMounted(async () => {
        await userStore.fetchCustomerId();
        cartStore.loadCart();
        cartStore.fetchDiscount();

        if (!userStore.customerId) {
          console.log("❌ customerId отсутствует, открываем CreateCustomerModal...");
          isCustomerModalOpen.value = true;
        }
      });

      // 🔄 Следим за `customerId`, если обновился — закрываем модальное окно
      watch(() => userStore.customerId, (newCustomerId) => {
        if (newCustomerId) {
          console.log("✅ customerId обновлён:", newCustomerId);
          isCustomerModalOpen.value = false;
        }
      });

      const cart = computed(() => cartStore.cart);
      const discount = computed(() => cartStore.discount);

      // ✅ Функция расчёта скидки
      const getDiscountedPrice = (price: number) => {
        return discount.value > 0
          ? (price * (1 - discount.value / 100)).toFixed(2)
          : price.toFixed(2);
      };

      // ✅ Итоговая сумма с учётом скидки
      const total = computed(() =>
        cart.value.reduce(
          (sum, item) => sum + parseFloat(getDiscountedPrice(item.price)) * item.quantity,
          0
        )
      );

      const removeFromCart = (id: string) => {
        cartStore.removeFromCart(id);
      };

      // ✅ Перед оформлением заказа проверяем `customerId`
      const handleOrder = async () => {
        //await userStore.fetchCustomerId();
        //console.log("⚡ handleOrder() проверяет customerId:", userStore.customerId);

        if (!userStore.customerId) {
          console.log("❌ customerId не найден...");
        } else {
          console.log("✅ customerId найден, отправляем заказ...");
          await cartStore.submitOrder();
          router.push("/orders");
        }
      };

      // ✅ После сохранения данных обновляем `customerId`
      const onCustomerSaved = (customer: { id: string }) => {
        console.log("✅ Новый customerId:", customer.id);
        userStore.setCustomerId(customer.id);
        isCustomerModalOpen.value = false;
      };

      return {
        cart,
        discount,
        total,
        removeFromCart,
        handleOrder,
        getDiscountedPrice,
        isCustomerModalOpen,
        onCustomerSaved,
      };
    },
  });
</script>

































