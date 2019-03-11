import * as React from 'react'
import Button from '@material-ui/core/Button';
import Swal from 'sweetalert2'
import { PerformLinkGroupSearch } from './LinkGroupFunctions'

interface IProps {
    className: string | undefined,
    linkGroupId: number
}

interface IState {

}

export class CopyLinkGroupButton extends React.Component<IProps, IState> {
    constructor(props: any) {
        super(props)
        this.CopyLinkGroupClick = this.CopyLinkGroupClick.bind(this);
    }


    CopyLinkGroupClick() {
        Swal({
            title: 'Enter name for the new Link Group',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Ok',
            showLoaderOnConfirm: true,
            inputAttributes: {
                autocapitalize: 'off'
            },

            preConfirm: (linkGroupName) => {
                
                let dataArray: any = []

                return new Promise((resolve, reject) => {
                    PerformLinkGroupSearch(linkGroupName, "", "", "","")
                        .then(result => {
                            dataArray = result;
                            if (dataArray.length === 0)
                                resolve(linkGroupName)
                            else
                                throw new Error("A Link Group already exists with that name.")

                        }).catch(error => {
                            Swal.showValidationMessage(error.message)
                            Swal.hideLoading();
                        });
                })

            },
            input: 'text'
        }).then(result => {
            if (result.dismiss === Swal.DismissReason.cancel) return;
            this.CopyLinkGroup(this.props.linkGroupId, result.value)
        }).catch(error => {
            this.CopyLinkGroupError(error)
        })
    }


    CopyLinkGroup(id: number, newLinkGroupName: string) {
        var message = "Copy LinkGroup Id: " + id + " with name: " + newLinkGroupName
        Swal({
            type: 'success',
            html: message
        })
    }

    CopyLinkGroupError(message: string) {
        Swal({
            type: 'error',
            html: message
        })
    }

    render() {
        return (
            <React.Fragment>
                <Button
                    variant="contained"
                    color="primary"
                    className={this.props.className}
                    onClick={this.CopyLinkGroupClick}
                >
                    Copy Link Group
                </Button>
            </React.Fragment>
        )
    }
}

export default CopyLinkGroupButton