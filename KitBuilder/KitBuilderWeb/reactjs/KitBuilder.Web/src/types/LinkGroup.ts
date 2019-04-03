export interface Item {
    itemId: number,
    scanCode: string,
    productDesc: string,
    brandName: string,
}

export interface LinkGroupItem {
    linkGroupId: number,
    linkGroupItemId: number,
    item: Item,
    instructionListId: number,
}

export interface LinkGroup {
    linkGroupId: number,
    groupName: string,
    groupDescription: string,
    itemScanCode: number | void,
    itemDescription: string | void,
    insertDateUtc: string,
    linkGroupItemDto: Array<LinkGroupItem>,
}