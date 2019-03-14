export interface Item {
    itemId: number,
    ScanCode: string,
    ProductDesc: string,
    BrandName: string,
}

export interface LinkedGroupItem {
    linkGroupId: number,
    linkGroupItemId: number,
    item: Item,
    selected? : boolean,
}

export interface LinkedGroup {
    linkGroupId: number,
    groupName: string,
    groupDescription: string,
    itemScanCode: number | void,
    itemDescription: string | void,
    insertDateUtc: string,
    linkGroupItemDto: Array<LinkedGroupItem>,
}