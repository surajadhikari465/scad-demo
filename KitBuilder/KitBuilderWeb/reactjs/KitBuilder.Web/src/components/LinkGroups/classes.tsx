

export class linkGroupItem {
    itemId: number
    scanCode: string
    productDesc: string
    posDesc: string
}

export class linkGroupItemDto {
    linkGroupItemId: number 
    linkGroupId: number
    item: Array<linkGroupItem>
    insertDateUtc: Date 
}

export class linkGroupDto {
    groupDescription: string
    groupName:string
    linkGroupId: number
    linkGroupItems: Array<linkGroupItemDto>
}


