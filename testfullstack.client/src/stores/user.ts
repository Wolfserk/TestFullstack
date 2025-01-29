import { defineStore } from "pinia";
import axios from "axios";

export const useUserStore = defineStore("user", {
  state: () => ({
    email: null as string | null,
    role: null as string | null,
    token: null as string | null,
    isAuthenticated: false,
  }),
  actions: {
    setUser(email: string, role: string, token: string) {
      this.email = email;
      this.role = role;
      this.token = token;
      this.isAuthenticated = true;

      localStorage.setItem("authToken", token);
      localStorage.setItem("userEmail", email);
      localStorage.setItem("userRole", role);
    },
    initializeUser() {
      const token = localStorage.getItem("authToken");
      const email = localStorage.getItem("userEmail");
      const role = localStorage.getItem("userRole");

      if (token && email && role) {
        this.email = email;
        this.role = role;
        this.token = token;
        this.isAuthenticated = true;
      } else {
        this.clearUser();
      }
    },
    clearUser() {
      this.email = null;
      this.role = null;
      this.token = null;
      this.isAuthenticated = false;
      localStorage.removeItem("authToken");
      localStorage.removeItem("userEmail");
      localStorage.removeItem("userRole");
      delete axios.defaults.headers.common["Authorization"];
    },
  },
});
