<template>
  <div>
    <h1>Our insurance plans</h1>
    <ul>
      <li v-for="plan in plans" :key="plan.id">
        {{ plan.name }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { inject, onMounted, ref } from 'vue';
import { PlansClient } from '@/clients/task-api-rest-client/plans-client';
import { Plan } from '@/models/plan';

const plans = ref([] as Plan[]);

onMounted(async () => {
  const client = inject('PlansClient') as PlansClient;
  try {
    plans.value = await client.getAll();
  } catch (error) {
    console.log(error);
  }
})

</script>
