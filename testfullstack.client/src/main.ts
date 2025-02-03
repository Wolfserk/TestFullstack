import './assets/tailwind.css';


import { createApp } from 'vue'
import App from './App.vue'
import router from './router';
import { createPinia } from "pinia";
import { useUserStore } from "./stores/user";




const app = createApp(App);
const pinia = createPinia();
const rout = router;

app.use(rout);
app.use(pinia);

const userStore = useUserStore();
userStore.initializeUser();


app.mount("#app");

