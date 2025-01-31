//import { defineStore } from "pinia";
//import { ref, watch, computed } from "vue";
//import { useUserStore } from "./user";
//import axios from "axios";

//export const useCartStore = defineStore("cart", () => {
//  const userStore = useUserStore();
//  const cart = ref<{ id: string; name: string; price: number; quantity: number }[]>([]);

//  // 🔹 Получаем ключ корзины для текущего пользователя
//  const getCartKey = () => `cart_${userStore.userId || "guest"}`;

//  // 🔹 Загружаем корзину текущего пользователя при запуске
//  const loadCart = () => {
//    const storedCart = localStorage.getItem(getCartKey());
//    cart.value = storedCart ? JSON.parse(storedCart) : [];
//  };

//  // 🔹 Следим за изменениями в корзине и сохраняем в `localStorage`
//  watch(cart, (newCart) => {
//    localStorage.setItem(getCartKey(), JSON.stringify(newCart));
//  }, { deep: true });

//  // 🔹 Добавление товара в корзину
//  const addToCart = (item: { id: string; name: string; price: number }) => {
//    const existingItem = cart.value.find(cartItem => cartItem.id === item.id);
//    if (existingItem) {
//      existingItem.quantity++;
//    } else {
//      cart.value.push({ ...item, quantity: 1 });
//    }
//  };

//  // 🔹 Удаление товара из корзины
//  const removeFromCart = (itemId: string) => {
//    cart.value = cart.value.filter(item => item.id !== itemId);
//  };

//  // 🔹 Очистка корзины
//  const clearCart = () => {
//    cart.value = [];
//    localStorage.removeItem(getCartKey());
//  };

//  // 🔹 Очищаем только state корзины при выходе (не удаляя данные)
//  const logoutCart = () => {
//    cart.value = [];
//  };

//  // 🔹 Подсчет количества товаров в корзине (для отображения)
//  const cartTotal = computed(() => cart.value.reduce((sum, item) => sum + item.quantity, 0));

//  // 🔹 Оформление заказа
//  const submitOrder = async () => {
//    if (cart.value.length === 0) {
//      alert("Ваша корзина пуста!");
//      return;
//    }

//    try {
//      const response = await axios.post(
//        "https://localhost:7034/api/orders",
//        { items: cart.value.map(item => ({ itemId: item.id, quantity: item.quantity, price: item.price })) },
//        { headers: { Authorization: `Bearer ${userStore.token}` } }
//      );

//      console.log("Заказ оформлен:", response.data);
//      clearCart();
//      alert("Ваш заказ успешно оформлен!");

//    } catch {
//      console.error("Ошибка при оформлении заказа!");
//      alert("Ошибка при оформлении заказа.");
//    }
//  };

//  return {
//    cart,
//    cartTotal,
//    addToCart,
//    removeFromCart,
//    clearCart,
//    loadCart,
//    logoutCart,
//    submitOrder
//  };
//});


import { defineStore } from "pinia";
import { ref, watch, computed } from "vue";
import { useUserStore } from "./user";
import axios from "axios";

export const useCartStore = defineStore("cart", () => {
  const userStore = useUserStore();
  const cart = ref<{ id: string; name: string; price: number; quantity: number }[]>([]);

  // 🟢 Получаем ключ корзины для текущего пользователя
  const getCartKey = () => `cart_${userStore.userId || "guest"}`;

  // 🟢 Загружаем корзину текущего пользователя при запуске
  const loadCart = () => {
    const storedCart = localStorage.getItem(getCartKey());
    cart.value = storedCart ? JSON.parse(storedCart) : [];
  };

  // 🟢 Следим за изменениями в корзине и сохраняем в `localStorage`
  watch(cart, (newCart) => {
    localStorage.setItem(getCartKey(), JSON.stringify(newCart));
  }, { deep: true });

  // 🟢 Добавление товара в корзину
  const addToCart = (item: { id: string; name: string; price: number }) => {
    const existingItem = cart.value.find(cartItem => cartItem.id === item.id);
    if (existingItem) {
      existingItem.quantity++;
    } else {
      cart.value.push({ ...item, quantity: 1 });
    }
  };

  // 🟢 Удаление товара из корзины
  const removeFromCart = (itemId: string) => {
    cart.value = cart.value.filter(item => item.id !== itemId);
  };

  // 🟢 Очистка корзины
  const clearCart = () => {
    cart.value = [];
    localStorage.removeItem(getCartKey());
  };

  // 🟢 Очищаем только state корзины при выходе (не удаляя данные)
  const logoutCart = () => {
    cart.value = [];
  };

  // 🟢 Подсчет количества товаров в корзине (для отображения)
  const cartTotal = computed(() => cart.value.reduce((sum, item) => sum + item.quantity, 0));

  // 🛑 **Перед отправкой заказа запрашиваем актуальные цены с сервера**
  const submitOrder = async () => {
    if (cart.value.length === 0) {
      alert("Ваша корзина пуста!");
      return;
    }

    try {
      // 🔹 Получаем актуальные цены с сервера
      const itemIds = cart.value.map(item => item.id);
      const { data: updatedItems } = await axios.post(
        "https://localhost:7034/api/items/get-prices",
        { itemIds },
        { headers: { Authorization: `Bearer ${userStore.token}` } }
      );

      // 🔹 Формируем обновленный заказ с правильными ценами
      const updatedCart = cart.value.map(cartItem => {
        const serverItem = updatedItems.find(item => item.id === cartItem.id);
        if (!serverItem) throw new Error(`Товар ${cartItem.name} не найден на сервере`);

        return {
          itemId: cartItem.id,
          quantity: cartItem.quantity,
          price: serverItem.price // ✅ Берем цену только с сервера
        };
      });

      // 🔹 Отправляем заказ
      await axios.post(
        "https://localhost:7034/api/orders",
        { items: updatedCart },
        { headers: { Authorization: `Bearer ${userStore.token}` } }
      );

      console.log("✅ Заказ оформлен успешно!");
      clearCart();
      alert("Ваш заказ успешно оформлен!");

    } catch (error) {
      console.error("❌ Ошибка при оформлении заказа:", error);
      alert("Ошибка при оформлении заказа.");
    }
  };

  return {
    cart,
    cartTotal,
    addToCart,
    removeFromCart,
    clearCart,
    loadCart,
    logoutCart,
    submitOrder
  };
});
