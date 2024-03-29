<template>
	<div class="inventory-container">
		<h1 id="inventoryTitle">
			Inventory Dashboard
		</h1>
		<hr />
		<inventory-chart> </inventory-chart>
		<div class="inventory-actions">
			<solar-button @buttonClick="showNewProductModal" id="addNewBtn">
				Add new item
			</solar-button>

			<solar-button
				@buttonClick="showShipmentModal"
				id="receiveShipmentBtn"
			>
				Receive Shipment
			</solar-button>
		</div>

		<table id="inventoryTable" class="table">
			<tr>
				<th>Item</th>
				<th>Quantity On-hand</th>
				<th>Unit Price</th>
				<th>Taxable</th>
				<th>Delete</th>
			</tr>

			<tr v-for="item in inventory" :key="item.id">
				<td>
					{{ item.product.name }}
				</td>
				<td
					v-bind:class="
						applyColor(item.quantityOnHand, item.idealQuantity)
					"
				>
					{{ item.quantityOnHand }}
				</td>
				<td>$ {{ item.product.price.toFixed(2) }}</td>
				<td>
					<span v-if="item.product.isTaxable">
						Yes
					</span>
					<span v-else>
						No
					</span>
				</td>
				<td>
					<div
						class="lni lni-cross-circle product-archive"
						@click="archiveProduct(item.product.id)"
					></div>
				</td>
			</tr>
		</table>

		<new-product-modal
			v-if="isNewProductVisible"
			@saveProduct="saveNewProduct"
			@close="closeModals"
		/>
		<shipment-modal
			v-if="isShipmentVisible"
			:inventory="inventory"
			@saveShipment="saveNewShipment"
			@close="closeModals"
		/>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProduct, IProductInventory } from "@/types/Product";
import SolarButton from "@/components/SolarButton.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import { IShipment } from "@/types/Shipment";
import { InventoryService } from "@/services/inventory-service";
import { ProductService } from "@/services/product-service";
import InventoryChart from "@/components/charts/inventoryChart.vue";

const inventoryService = new InventoryService();
const productService = new ProductService();

@Component({
	name: "Inventory",
	components: { SolarButton, InventoryChart, NewProductModal, ShipmentModal }
})
export default class Inventory extends Vue {
	isNewProductVisible = false;
	isShipmentVisible = false;

	inventory: IProductInventory[] = [];

	async created() {
		await this.initialize();
	}

	applyColor(current: number, targen: number) {
		if (current <= 0) {
			return "red";
		}
		if (Math.abs(targen - current) > 8) {
			return "yellow";
		}
		return "green";
	}

	showNewProductModal() {
		this.isNewProductVisible = true;
	}

	showShipmentModal() {
		this.isShipmentVisible = true;
	}

	closeModals() {
		this.isShipmentVisible = false;
		this.isNewProductVisible = false;
	}

	async archiveProduct(productId: number) {
		await productService.archive(productId);
		await this.initialize();
	}

	async saveNewProduct(newProduct: IProduct) {
		await productService.save(newProduct);
		this.isNewProductVisible = false;
		await this.initialize();
	}

	async saveNewShipment(shipment: IShipment) {
		// const product = this.inventory.find(p => p.id == shipment.productId);

		// if (product) {
		// 	this.$set(product, "quantityOnHand", shipment.adjustment);
		// }
		await inventoryService.updateInventoryQuantity(shipment);
		this.isShipmentVisible = false;
		await this.initialize();
	}

	async initialize() {
		this.inventory = await inventoryService.getInventory();
		await this.$store.dispatch("assignSnapshots");
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.green {
	font-weight: bold;
	color: $solar-green;
}

.yellow {
	font-weight: bold;
	color: $solar-yellow;
}

.red {
	font-weight: bold;
	color: $solar-red;
}

.inventory-actions {
	display: flex;
	margin-bottom: 0.8rem;
}

.product-archive {
	cursor: pointer;
	font-weight: bold;
	font-size: 1.2rem;
	color: $solar-red;
}
</style>
