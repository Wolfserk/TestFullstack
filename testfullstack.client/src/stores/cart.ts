import { defineStore } from "pinia";

export const useCartStore = defineStore("cart", {
  state: () => ({
    cart: [] as { id: string; name: string; price: number; quantity: number }[],
  }),
  actions: {
    addToCart(item: { id: string; name: string; price: number }, quantity: number) {
      const existingItem = this.cart.find((i) => i.id === item.id);
      if (existingItem) {
        existingItem.quantity += quantity;
      } else {
        this.cart.push({ ...item, quantity });
      }
    },
    removeFromCart(id: string) {
      this.cart = this.cart.filter((item) => item.id !== id);
    },
    clearCart() {
      this.cart = [];
    },
  },
});
