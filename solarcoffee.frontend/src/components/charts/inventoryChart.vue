<template>
	<div v-if="timelineBuilt">
		<apexchart
			type="area"
			:width="'100%'"
			height="300"
			:options="options"
			:series="series"
		>
		</apexchart>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Get, Sync } from "vuex-pathify";
import { IInventoryTimeline } from "@/types/InventoryGraph";
import VueApexCharts from "vue-apexcharts";
import moment from "moment";
import { ApexOptions } from "apexcharts";

@Component({ name: "InventoryChart", components: { apexchart: VueApexCharts } })
export default class InventoryChart extends Vue {
	@Sync("snapshotTimeline")
	snapshotTimeline!: IInventoryTimeline;

	@Get("timelineBuilt")
	timelineBuilt?: boolean;

	get options(): ApexOptions {
		return {
			dataLabels: { enabled: false },
			fill: {
				type: "gradient"
			},
			stroke: {
				curve: "smooth"
			},
			xaxis: {
				categories: this.snapshotTimeline.timeline.map(t =>
					moment(t).format("DD HH:mm:ss")
				),
				type: "datetime"
			}
		};
	}

	get series() {
		const result = this.snapshotTimeline.productInventorySnapshots.map(
			snapshot => ({
				name: snapshot.productId.toString(),
				data: snapshot.quantityOnHand
			})
		);
		return result;
	}
}
</script>

<style scoped lang="scss"></style>
