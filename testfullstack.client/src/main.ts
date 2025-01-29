/*import './assets/main.css'*/
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

// Восстанавливаем пользователя при загрузке приложения
const userStore = useUserStore();
userStore.initializeUser();


app.mount("#app");


//const userStore = useUserStore();
//userStore.initializeUser();

//createApp(App).use(router).use(createPinia()).mount('#app');
