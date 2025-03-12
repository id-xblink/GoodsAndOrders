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
                <button v-if="authStore.isAdmin && row.status === 'Новый'" class="btn begin" @click="$emit('begin', row)">
                  <Package class="icon" />
                </button>

                <button v-if="authStore.isAdmin && row.status === 'Выполняется' && row.shipmentDate && formatDate(row.shipmentDate) <= formatDate()"
                  class="btn finish" @click="$emit('finish', row)">
                  <CheckCircle class="icon" />
                </button>
                
                <button v-if="row.status === 'Новый'" class="btn delete" @click="$emit('delete', row.id)">
                  <Trash2 class="icon" />
                </button>

                <button class="btn check" @click="$emit('check', row)">
                  <Search class="icon" />
                </button>
              </template>

              <template v-else-if="col === 'shipmentDate'">
                {{ row[col] ? formatDate(row[col]) : "-" }}
              </template>

              <template v-else-if="col === 'orderDate'">
                {{ formatDate(row[col]) }}
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
import { Package, CheckCircle, Trash2, Search } from 'lucide-vue-next';
const authStore = useAuthStore();

defineProps({
  columns: Array,
  data: Array,
});

const columnAliases = {
  orderNumber: "Номер заказа",
  orderDate: "Дата заказа",
  status: "Статус",
  shipmentDate: "Дата доставки",
  actions: "Действия",
};

// Форматирование даты
const formatDate = (date) => {
  return new Date(date || Date.now()).toLocaleDateString();
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
  .btn.begin {
    background: #b9c239;
    color: white;
  }
  .btn.finish {
    background: #1abc9c;
    color: white;
  }
  .btn.delete {
    background: #e74c3c;
    color: white;
  }
  .btn.check {
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