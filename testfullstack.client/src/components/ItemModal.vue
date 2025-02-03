<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <header class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">{{ title }}</h2>
        <button @click="close" class="text-gray-500 hover:text-gray-700">&times;</button>
      </header>
      <main>
        <form>
          <label class="block mb-2">Код товара</label>
          <input v-model="item.code"
                 @input="formatCode"
                 type="text"
                 class="w-full p-2 mb-2 border border-gray-300 rounded-lg"
                 placeholder="XX-XXXX-XXYY" />
          <p v-if="!isCodeValid" class="text-red-500 text-sm mb-2">Формат: XX-XXXX-YYXX</p>

          <label class="block mb-2">Название</label>
          <input v-model="item.name" type="text" class="w-full p-2 mb-4 border border-gray-300 rounded-lg" />

          <label class="block mb-2">Цена</label>
          <input v-model="item.price" type="number" class="w-full p-2 mb-4 border border-gray-300 rounded-lg" />

          <label class="block mb-2">Категория</label>
          <input v-model="item.category" type="text" class="w-full p-2 mb-2 border border-gray-300 rounded-lg" />
          <p v-if="!isCategoryValid" class="text-red-500 text-sm mb-4">Максимальная длина — 30 символов</p>
        </form>
      </main>
      <footer class="mt-4 flex justify-end">
        <button @click="close" class="bg-gray-300 px-4 py-2 rounded-lg hover:bg-gray-400">Отмена</button>
        <button @click="onSubmit"
                :disabled="!isCodeValid || !isCategoryValid"
                class="bg-blue-600 text-white px-4 py-2 ml-2 rounded-lg hover:bg-blue-700 disabled:bg-gray-400">
          Сохранить
        </button>
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent, reactive, watch, computed } from "vue";

  export default defineComponent({
    name: "ItemModal",
    props: {
      isOpen: Boolean,
      title: String,
      itemData: Object,
    },
    emits: ["close", "submit"],
    setup(props, { emit }) {
      const item = reactive({ code: "", name: "", price: 0, category: "" });

      watch(
        () => props.itemData,
        (newData) => {
          if (newData) {
            Object.assign(item, newData);
          }
        },
        { deep: true, immediate: true }
      );

      const codePattern = /^\d{2}-\d{4}-[A-Z]{2}\d{2}$/;
      const isCodeValid = computed(() => codePattern.test(item.code));

      const isCategoryValid = computed(() => item.category.length <= 30);

      const close = () => emit("close");
      const onSubmit = () => {
        if (isCodeValid.value && isCategoryValid.value) {
          emit("submit", { ...item });
        }
      };

      const formatCode = (event: Event) => {
        const input = event.target as HTMLInputElement;
        let value = input.value.replace(/-/g, ""); // Убираем все дефисы
        if (value.length > 2) value = value.slice(0, 2) + "-" + value.slice(2);
        if (value.length > 7) value = value.slice(0, 7) + "-" + value.slice(7);
        item.code = value.toUpperCase();
      };

      return { item, isCodeValid, isCategoryValid, close, onSubmit, formatCode };
    },
  });
</script>
