<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md">
      <header class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">{{ title }}</h2>
        <button @click="close" class="text-gray-500 hover:text-gray-700">&times;</button>
      </header>
      <main>
        <slot></slot>
      </main>
      <footer class="mt-4 flex justify-end">
        <button @click="close" class="bg-gray-300 px-4 py-2 rounded-lg hover:bg-gray-400">Отмена</button>
        <button @click="onSubmit"
                class="bg-blue-600 text-white px-4 py-2 ml-2 rounded-lg hover:bg-blue-700">
          Сохранить
        </button>
      </footer>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from "vue";

  export default defineComponent({
    name: "UserModal",
    props: {
      isOpen: { type: Boolean, required: true },
      title: { type: String, required: true },
    },
    emits: ["close", "submit"],
    methods: {
      close() {
        this.$emit("close");
      },
      onSubmit() {
        this.$emit("submit");
      },
    },
  });
</script>

<style scoped>
  body {
    overflow: hidden;
  }
</style>
