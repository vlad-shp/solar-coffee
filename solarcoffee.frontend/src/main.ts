import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import moment from "moment";

Vue.config.productionTip = false;

Vue.filter("humanizeDate", function(date: Date) {
	return moment(date).format("MMMM Do YYYY");
});

Vue.filter("price", function(price: number) {
	return price.toFixed(2);
});

new Vue({
	router,
	store,
	render: h => h(App)
}).$mount("#app");
