import styled from 'styled-components';

export const RegionContainer = styled.div`
    flex-direction: column;
    margin: 5px;
    padding: 5px;
    flex: 1;
    height: calc(100vh - 195px);
    overflow-y: scroll;
`

export const ToggleGroup = styled.div`
    display: flex;
    flex-direction: column;
    .flex-container{
        .wfm-button{
            display: flex;
            flex: 1;
        }
    }
`