export interface IInventoryTimeline {
	productInventorySnapshots: ISnapshotsp[];
	timeline: Date[];
}

export interface ISnapshot {
	productId: number;
	quantityOnHand: number[];
}
