import { defineStore } from "pinia";
import { ref, watch, computed } from "vue";
import { useUserStore } from "./user";
import axios from "axios";

export const useCartStore = defineStore("cart", () => {
  const userStore = useUserStore();
  const cart = ref<{ id: string; name: string; price: number; quantity: number }[]>([]);
  const discount = ref<number>(0);

  const getCartKey = () => `cart_${userStore.userId || "guest"}`;

  const loadCart = () => {
    const key = getCartKey();
    const storedCart = localStorage.getItem(key);
    cart.value = storedCart ? JSON.parse(storedCart) : [];
  };

  watch(cart, () => {
    localStorage.setItem(getCartKey(), JSON.stringify(cart.value));
  }, { deep: true });

  const switchUserCart = () => {
    const oldCartKey = getCartKey(); 
    const oldCart = JSON.parse(localStorage.getItem(oldCartKey) || "[]");

    cart.value = [];

    loadCart();

    if (cart.value.length === 0 && oldCart.length > 0 && userStore.userId) {
      cart.value = oldCart;
    }

    if (userStore.userId) {
      localStorage.removeItem("cart_guest");
    }
  };

  const fetchDiscount = async () => {
    if (!userStore.token) return;
    try {
      const { data } = await axios.get("https://localhost:7034/api/customers/discount", {
        headers: { Authorization: `Bearer ${userStore.token}` },
      });
      discount.value = data.discount || 0;
    } catch {
      discount.value = 0;
    }
  };

  const addToCart = (item: { id: string; name: string; price: number }, quantity: number) => {
    const existingItem = cart.value.find(cartItem => cartItem.id === item.id);
    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      cart.value.push({ ...item, quantity });
    }
  };

  const removeFromCart = (itemId: string) => {
    cart.value = cart.value.filter(item => item.id !== itemId);
  };

  const clearCart = () => {
    cart.value = [];
  };

  const cartTotal = computed(() => cart.value.reduce((sum, item) => sum + item.quantity, 0));

  const submitOrder = async () => {
    if (cart.value.length === 0) {
      alert("Ваша корзина пуста!");
      return;
    }

    try {
      const itemIds = cart.value.map(item => item.id);
      const { data: updatedItems } = await axios.post(
        "https://localhost:7034/api/items/get-prices",
        { itemIds },
        { headers: { Authorization: `Bearer ${userStore.token}` } }
      );

      const updatedCart = cart.value.map(cartItem => {
        const serverItem = updatedItems.find((item: { id: string; }) => item.id === cartItem.id);
        if (!serverItem) {
          alert(`${cartItem.name} на данный момент отсутствует в каталоге. Если хотите оформить заказ, нужно убрать данный товар из корзины.`);
          throw new Error(`Товар ${cartItem.name} не найден на сервере`);
        }

        return {
          itemId: cartItem.id,
          quantity: cartItem.quantity,
          price: discount.value > 0
            ? (serverItem.price * (1 - discount.value / 100)).toFixed(2)
            : serverItem.price.toFixed(2),
        };
      });

      await axios.post(
        "https://localhost:7034/api/orders",
        { items: updatedCart },
        { headers: { Authorization: `Bearer ${userStore.token}` } }
      );

      console.log("Заказ оформлен успешно!");
      clearCart();
      alert("Ваш заказ успешно оформлен!");

    } catch (error) {
      console.error("Ошибка при оформлении заказа:", error);
      alert("Ошибка при оформлении заказа.");
    }
  };

  return {
    cart,
    cartTotal,
    discount,
    addToCart,
    removeFromCart,
    clearCart,
    loadCart,
    submitOrder,
    fetchDiscount,
    switchUserCart,
  };
});
