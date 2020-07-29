<template>
    <div class="chargeback-detail padding-top">
        <mt-header fixed title="退单详情">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>   
        <div class="content">
            <div class="chargecack-flex1">
                <div class="chargeback-title">
                    <p>退单原因：</p>
                    <p>退单原因描述：</p>
                    <p>退单时间：</p>
                </div>
                <div class="chargeback-content">
                    <p>{{recordDetail.refundReason}}</p>
                    <p>{{recordDetail.refundRemarks}}</p>
                    <p>{{recordDetail.createdOn}}</p>
                    <i class="mint-cell-allow-right"></i>
                </div>     
            </div>
            <div class="lookOrder">
                <router-link :to="{path: '/order-look', query: {id: recordDetail.consultID}}" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">查看咨询详情</mt-button>
                </router-link>
            </div>
            
        </div> 
        <ul v-if="checkState === 'checking'" class="navBar">
            <li @click="clickStar('确认通过退单？', 1)"><a class="green-hint" href="javascript:;">通过</a></li>
            <li @click="clickStar('确认不通过退单？', 0)"><a class="red-hint" href="javascript:;">不通过</a></li>
        </ul>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            id: 0,
            recordDetail: {},
            checkState: ''
        }
    },
    methods: {
        clickStar(mes, index) {
            MessageBox.confirm(mes).then(action => {
                this.submitAudit(index)
            },function(){
                console.log('取消了');
            })
        },
        getOrderList() { // 获取退单列表
            const id = this.id 
            this.instance.refundManageByIdA({
                id
            })
                .then((response) => {
                    this.recordDetail = response.data.result.item
                    this.recordDetail.createdOn = moment(this.recordDetail.createdOn).format('YYYY-MM-DD HH:mm:ss')
                    this.checkState = this.recordDetail.checkState
                    if (this.recordDetail.refundReason) {
						this.recordDetail.refundReason = JSON.parse(this.recordDetail.refundReason).LabelName
					}
                })
                .catch((error) => {
                }) 
        },
        getParams() { // 请求审核类型
            this.instance.yaeherPatientParameterListD({
                type:'ConfigPar',
                systemCode: 'CheckType', 
            })
                .then((response) => {
                    this.paramsList = response.data.result.item
                })
                .catch((error) => {
                }) 
        },
        submitAudit(index) { // 提交审核
            const id = this.id
            const checkState = this.paramsList[index].code
            this.instance.updateRefundManageA({
                id,
                checkState
            })
                .then((response) => {
                    console.log(response.data.result.code)
                    if (response.data.result.code === 200) {
                        Toast('操作成功')
                        this.$router.go(-1)
                    }
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getOrderList()
        this.getParams()
    },
    created () {
        this.id = this.$route.query.id
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.chargeback-detail {
    .chargecack-flex1 {
        display: flex;
        font-size: $font-l;
        line-height: 30px;
        background: $color-wfont;
        padding: 10px;
        p {
            padding-bottom: 5px;
            min-height: 19px;
        }
        .chargeback-title {
            width: 100px;
        }
        .chargeback-content {
            flex: 1;
            .mint-cell-allow-right::after {
                right: 0;
            }
        }
    }

    .lookOrder {
        margin: 30px 10px;
    }
}

</style>