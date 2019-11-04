import { InstructionList } from './InstructionList';

export interface Item {
    itemId: number,
    scanCode: string,
    productDesc: string,
    posDesc: string,
    brandName: string,
}

export interface LinkGroupItem {
    linkGroupId: number,
    linkGroupItemId: number,
    item: Item,
    instructionListId: number,
    instructionList: InstructionList,
}

export interface LinkGroup {
    linkGroupId: number,
    groupName: string,
    groupDescription: string,
    itemScanCode: number | void,
    itemDescription: string | void,
    itemPosDesc: string | void,
    insertDateUtc: string,
    linkGroupItemDto: Array<LinkGroupItem>,
}
export interface LinkGroupItemId {
    linkGroupId: number
    
}