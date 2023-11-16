import { createApp } from "vue";
import { createPinia } from 'pinia';
import App from "./App.vue";
import router from "./router";
import config from './appsettings';
import { PlansClient } from "./clients/task-api-rest-client/plans-client";
import { CompaniesClient } from "./clients/task-api-rest-client/companies-client";

const pinia = createPinia();
const app = createApp(App);
app.provide('PlansClient', new PlansClient(config.taskApiRestClientOptions));
app.provide('CompaniesClient', new CompaniesClient(config.taskApiRestClientOptions));

app.use(pinia).use(router).mount("#app");
