<template>
    <table class="table">
      <thead>
        <tr>
          <th v-for="col in columns" :key="col">
            {{ columnAliases[col] || col }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in data" :key="row.id">
          <td v-for="col in columns" :key="col">
            <slot :name="col" :row="row">
              <template v-if="col === 'actions'">
                <button v-if="authStore.isAdmin" class="btn edit" @click="$emit('edit', row)">
                  <Pencil class="icon" />
                </button>
                <button v-if="authStore.isAdmin" class="btn delete" @click="$emit('delete', row.id)">
                  <Trash2 class="icon" />
                </button>
              </template>
              <template v-else>
                {{ row[col] }}
              </template>
            </slot>
          </td>
        </tr>
      </tbody>
    </table>
  </template>
  
  <script setup>
    import { useAuthStore } from '@/stores/authStore';
    import { Pencil, Trash2 } from 'lucide-vue-next';
    const authStore = useAuthStore();
  
    defineProps({
      columns: Array,
      data: Array,
    });
  
    const columnAliases = {
      name: "Имя",
      login: "Логин",
      code: "Код",
      address: "Адрес",
      discount: "Скидка",
      userRole: "Роль",
      actions: "Действия",
    };
  </script>
  
  <style scoped>
    .table {
      width: 100%;
      border-collapse: collapse;
      margin-top: 20px;
    }
    .table th, .table td {
      border: 1px solid #ddd;
      padding: 10px;
      text-align: left;
    }
    .table th {
      background: #2c3e50;
      color: white;
    }
    .btn {
      border: none;
      cursor: pointer;
      padding: 5px 8px;
      margin-right: 5px;
      font-size: 14px;
      border-radius: 4px;
    }
    .btn.edit {
      background: #b9c239;
      color: white;
    }
    .btn.delete {
      background: #e74c3c;
      color: white;
    }
    .btn.cart {
      background: #3498db;
      color: white;
    }
    .btn:hover {
      opacity: 0.8;
    }
    .icon {
      width: 24px;
      height: 24px;
      display: flex;
      justify-content: center;
      align-items: center;
    }
  </style>
  