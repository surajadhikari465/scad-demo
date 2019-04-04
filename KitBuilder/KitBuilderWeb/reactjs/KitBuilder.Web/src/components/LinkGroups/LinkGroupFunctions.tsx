import axios from 'axios'
import { KbApiMethod } from '../helpers/kbapi';

export function PerformLinkGroupSearch(groupName: string, groupDesc: string, modifierDesc: string, modifierPlu: string, regions: string) {
    return new Promise((resolve, reject) => {
        axios.get(KbApiMethod("LinkGroupsSearch"), {
            params: {
                LinkGroupName: groupName,
                LinkGroupDesc: groupDesc,
                modifierDesc: modifierDesc,
                ModifierPlu: modifierPlu,
                Regions: regions
            }
        }).then(res => {
            resolve(res.data);
        }).catch(error => {
            reject(error);
        });
    })
}

export function PerformLinkGroupModifierSearch(modifierPlu: string, modifierName: string) {
    return new Promise((resolve, reject) => {
        axios.get(KbApiMethod("LinkGroupItemSearch"), {
            params: {
                ModiferPlu: modifierPlu,
                ModifierName: modifierName
            }
        }).then(res => {
            resolve(res.data);
        }).catch(error => {
            reject(error);
        });
    })
}

export function LoadCookingInstructions() {
    return new Promise((resolve, reject) => {
        axios.get(KbApiMethod("InstructionListByType"), {
            params: {
                instructionListType: "Cooking"
            }
        })
            .then(res => {
                resolve(res.data)
            }).catch(error => {
                reject(error)
            });
    })
}