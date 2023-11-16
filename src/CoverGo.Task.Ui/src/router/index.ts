import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";
import HomeView from "../views/HomeView.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/plans",
    name: "plans",
    component: () =>
      import(/* webpackChunkName: "plans" */ "../views/PlansView.vue"),
  },
  {
    path: "/companies",
    name: "companies",
    component: () =>
      import(/* webpackChunkName: "companies" */ "../views/CompaniesView.vue"),
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
