import { defineStore } from "pinia";
import axios from "axios";

export const useUserStore = defineStore("user", {
  state: () => ({
    userId: null as string | null,
    email: null as string | null,
    role: null as string | null,
    token: null as string | null,
    customerId: null as string | null,
    isAuthenticated: false,
  }),
  actions: {
    setUser(userId: string, email: string, role: string, token: string, customerId: string) {
      this.email = email;
      this.role = role;
      this.token = token;
      this.customerId = customerId;
      this.isAuthenticated = true;

      localStorage.setItem("userId", userId);
      localStorage.setItem("authToken", token);
      localStorage.setItem("userEmail", email);
      localStorage.setItem("userRole", role);
      localStorage.setItem("userCustomerId", customerId);
    },
    initializeUser() {
      const userId = localStorage.getItem("userId");
      const token = localStorage.getItem("authToken");
      const email = localStorage.getItem("userEmail");
      const role = localStorage.getItem("userRole");
      const customerId = localStorage.getItem("userCustomerId");

      if (token && email && role && userId) {
        this.email = email;
        this.userId = userId;
        this.role = role;
        this.token = token;
        this.customerId = customerId;
        this.isAuthenticated = true;
      } else {
        this.clearUser();
      }
    },
        // ✅ Устанавливаем `customerId`
        setCustomerId(customerId: string) {
          this.customerId = customerId;
          localStorage.setItem("userCustomerId", customerId);
        },

        // ✅ Автоматический запрос customerId после входа
        async fetchCustomerId() {
          if (!this.token || this.customerId) return;

          try {
            const response = await axios.get("https://localhost:7034/api/customers/my", {
              headers: { Authorization: `Bearer ${this.token}` },
            });

            if (response.data && response.data.customerId) {
              //console.log("Получен customerId:", response.data.customerId);
              this.setCustomerId(response.data.customerId);
            }
          } catch (error) {
            console.error("Ошибка при получении customerId:", error);
          }
        },


    clearUser() {
      this.userId = null;
      this.email = null;
      this.role = null;
      this.token = null;
      this.isAuthenticated = false;
      localStorage.removeItem("userId");
      localStorage.removeItem("authToken");
      localStorage.removeItem("userEmail");
      localStorage.removeItem("userRole");
      localStorage.removeItem("userCustomerId");
      delete axios.defaults.headers.common["Authorization"];
    },
  },
});
