<template>
	<div>
		<h1 id="ordersTitle">
			Sales Orders
		</h1>
		<hr />
		<table id="sales-orders" class="table" v-if="orders.length">
			<thead>
				<tr>
					<th>CustomerId</th>
					<th>OrderId</th>
					<th>Order Total</th>
					<th>Order Status</th>
					<th>Mark Complete</th>
				</tr>
				<tr v-for="order in orders" :key="order.id">
					<td>{{ order.customer.id }}</td>
					<td>{{ order.id }}</td>
					<td>{{ getTotal(order) | price }}</td>
					<td :class="{ green: order.isPaid }">
						{{ getStatus(order.isPaid) }}
					</td>
					<td>
						<div
							v-if="!order.isPaid"
							class="lni lni-checkmark-circle
						order-complete"
							@click="markComplete(order.id)"
						></div>
					</td>
				</tr>
			</thead>
		</table>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { OrderService } from "@/services/order-service";
import { ISalesOrder } from "@/types/SalesOrder";

const orderService = new OrderService();

@Component
export default class Orders extends Vue {
	orders: ISalesOrder[] = [];

	async created() {
		this.initialize();
	}

	getStatus(isPaid: boolean) {
		return isPaid ? "Paid in Full" : "Unpaid";
	}

	getTotal(order: ISalesOrder) {
		return order.salesOrderItems.reduce(
			(a, b) => a + b["product"]["price"] * b["quantity"],
			0
		);
	}

	async initialize() {
		this.orders = await orderService.getOrders();
	}

	async markComplete(id: number) {
		await orderService.markOrderComplete(id);
		await this.initialize();
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
.order-complete {
	cursor: pointer;
	font-weight: bold;
	font-size: 1.2rem;
	color: $solar-green;
}
.green {
	font-weight: bold;
	color: $solar-green;
}
</style>
