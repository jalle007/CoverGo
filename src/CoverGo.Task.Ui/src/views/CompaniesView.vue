<template>
  <div>
    <h1>Our client companies</h1>
    <ul>
      <li v-for="company in companies" :key="company.id">
        {{ company.name }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { inject, onMounted, ref } from 'vue';
import { CompaniesClient } from '@/clients/task-api-rest-client/companies-client';
import { Company } from '@/models/company';

const companies = ref([] as Company[]);

onMounted(async () => {
  const client = inject('CompaniesClient') as CompaniesClient;
  try {
    companies.value = await client.getAll();
  } catch (error) {
    console.log(error);
  }
})
</script>
