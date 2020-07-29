<template>
    <div class="apply-contol padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="qualityAgree">
                <p class="dotP">平台的发展离不开质量控制，更好的质控能让怡禾上的每一位医生有更好的口碑和收入，质控委员对医生是一种荣誉，同时怡禾也会为质控工作支付一些酬劳。
质控委员将承担一下工作：</p>
                <p>1.复核低星咨询的咨询质量（平均每月数个）</p>
                <p>2.审核新上线医生的咨询质量</p>
                <p>3.审核相关专科拟分享咨询案例的质量</p>
                <p>4.审核相关专科的科普文章、课程的知识点</p>
                <p class="dotP">熟悉怡禾操作规范的要求，并符合以下条件的医生，可申请为医生质控委员：</p>
                <p>1.在平台提供咨询服务超过半年</p>
                <p>2.咨询总量超过300单</p>
                <p>3.评分不低于4.9分</p>
                <!-- <div class="auditHint" v-if="!isQuality && applyDetail ==='checking'">
                    申请质控委员审核状态：<span class="red-hint hint">审核中</span>
                </div>
                <div class="auditHint" v-if="isQuality && applyDetail ==='checking'">
                    取消质控委员审核状态：<span class="red-hint hint">审核中</span>
                </div> -->
                <div class="btnCase" v-if="applyDetail ==='checking'">
                    <mt-button type="default" class="mint-button--large">审核中</mt-button>
                </div>
                <div class="btnCase" v-if="!isQuality && applyDetail !='checking'">
                    <mt-button @click="applyControl(true, '申请提交成功')" type="primary" class="mint-button--large">提交申请</mt-button>
                </div>
                <div class="btnCase"  v-if="isQuality && applyDetail !='checking'">
                    <mt-button @click="applyControl(false, '取消提交成功')" type="primary" class="mint-button--large">取消质控委员</mt-button>
                </div>
            </div>
            <!-- <div v-if="isQuality">
                <p class="hintTop" v-if="applyState1 != 'qualitystart'">取消质控委员审核状态：
                    <span v-if="applyDetail ==='checking'" class="red-hint hint">审核中</span>
                    <span v-if="applyDetail ==='fail'" class="red-hint hint">未通过</span>
                </p>
                <p v-if="applyDetail ==='fail'" class="hintTop">不通过原因：{{applyList.checkRemark}}</p>
                <p class="hintTop" v-if="applyState1 === 'qualitystart'">申请质控委员审核状态：
                    <span v-if="applyDetail ==='success'" class="green-hint hint">已通过</span>
                </p>
                <div class="mui-input-row apply-row">
                    <p class="green-hint">您已成为质控委员!</p>
                </div>
                <div v-if="applyDetail !='checking'" class="mui-input-row apply-row">
                    <p class="apply-title">取消委员申请</p> -->
                    <!-- <div class="illness" @click="selectIllness">
                        <div class="illness-case">
                            <p class="illness-label">选择科室：</p>
                            <p v-if="iIInessType === ''" class="illness-value">请选择科室</p>
                            <p  v-if="iIInessType != ''" class="illness-value">{{iIInessType}}</p>
                        </div>
                    </div> -->
                    <!-- <p class="illness-label">填写原因：</p>
                    <mt-field label="取消质控原因" @input = "descInput" placeholder="填写原因" type="textarea" rows="5" :attr="{ maxlength: maxReplyLength }" v-model="applyRemark"></mt-field>
                    <span class="font-number">可输入{{remark}}字</span>
                </div>
                <div class="btnCase"  v-if="applyDetail !='checking'">
                    <mt-button @click="applyControl(false, '取消提交成功')" type="primary" class="mint-button--large">取消质控委员</mt-button>
                </div>
            </div>
            <div v-if="!isQuality">
                <p class="hintTop" v-if="applyState1 === 'qualitystart'">申请质控委员审核状态：
                    <span v-if="applyDetail ===''" class="red-hint hint">未申请</span>
                    <span v-if="applyDetail ==='checking'" class="red-hint hint">审核中</span>
                    <span v-if="applyDetail ==='fail'" class="red-hint hint">未通过</span>
                </p>
                <p v-if="applyDetail ==='fail'" class="hintTop">不通过原因：{{applyList.checkRemark}}</p>
                <p class="hintTop" v-if="applyState1 === 'qualitystop'">取消质控委员审核状态：
                    <span v-if="applyDetail ==='success'" class="green-hint hint">已通过</span>
                </p>
                <div v-if="applyDetail !='checking'" class="mui-input-row apply-row">
                    <p class="apply-title">申请质控委员</p>
                    <p class="illness-label">填写原因：</p>
                    <mt-field label="申请质控原因" @input = "descInput" placeholder="填写原因" type="textarea" rows="5" :attr="{ maxlength: maxReplyLength }" v-model="applyRemark"></mt-field>
                    <span class="font-number">可输入{{remark}}字</span>
                </div>
                <div class="btnCase" v-if="applyDetail !='checking'">
                    <mt-button @click="applyControl(true, '申请提交成功')" type="primary" class="mint-button--large">提交</mt-button>
                </div>
            </div> -->

            <!-- <mt-picker :slots="dataList" ref="picker" :showToolbar="true" v-show="show">
                <div @click="selectIllness" class="slots-no">取消</div>
                <div @click="getPickerValue" class="slots-ok">确认</div>
            </mt-picker> -->

        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { createSecret } from 'assets/js/common.js'
export default {
    data () {
        return {
            show: false,
            applyRemark: '',
            selected: 'nav3',
            iIInessType: '',
            maxReplyLength: 300,
            remark: 300,
            id: 0,
            slotsArry: [],
            userdetail: {},
            applyParams: [],
            applyList: {},
            clinicID: -1,
            isQuality: '',
            applyDetail: '',
            applyState1: '',
            title: ''
        }
    },
    methods: {
        descInput() {
            let txtVal = this.applyRemark.length;
            this.remark = this.maxReplyLength - txtVal;
        },
        // selectIllness() { // 呼出科室选择
        //     this.show = !this.show
        // },
        getPickerValue() { // 选择科室
            this.show = !this.show
            this.iIInessType = this.$refs.picker.getValues()[0]                
            for (let i = 0 ;i < this.clinicList.length; i++) {
                if(this.clinicList[i].clinicName === this.iIInessType) {
                    this.clinicID = this.clinicList[i].id
                    console.log(this.clinicID)
                    return
                } else {
                    this.clinicID = -1
                }
            }
        },
        getUserInfo() { // 请求科室列表
            this.instance.clinicListD({
            })
                .then((response) => {
                    this.clinicList = response.data.result.item
                    console.log(this.clinicList)
                    for(var j = 0;j < this.clinicList.length;j++ ) {
                        this.slotsArry.push(this.clinicList[j].clinicName)
                    }
                })
                .catch((error) => {
                }) 
        },
        getstate() { // 申请状态
            this.instance.qualityCommitteeByUserD({
            })
                .then((response) => {
                    if (response.data.result.item) {
                        this.applyList =  response.data.result.item[0]
                        this.applyDetail = response.data.result.item[0].checkState
                        // this.applyState1 = response.data.result.item[0].applyState
                    }
                    
                    console.log(this.applyState1, this.applyDetail)
                    
                })
                .catch((error) => {
                }) 
        },
        getParams() { // 申请参数
            this.instance.yaeherPatientParameterListD({
                type:'ConfigPar',
                systemCode: 'QualityState'
            })
                .then((response) => {
                    this.applyParams = response.data.result.item
                })
                .catch((error) => {
                }) 
        },
        applyControl(flag, msg) { // 申请质控委员
            // const applyRemark = this.applyRemark
            let applyState = ''
            if (flag) {
                applyState = this.applyParams[0].code
            } else {
                // 取消质控委员
                applyState = this.applyParams[1].code
            }
            
            // if(!applyRemark) {
            //     Toast('申请原因不能为空')
            //     return
            // }
            this.instance.createQualityCommitteeRegisterD({
                applyRemark: '',
                applyState
            })
                .then((response) => {
                    if(response.data.result.code === 200) {
                        Toast(msg)
                        this.$router.push({ 
                            path: '/doctor-user'
                        })
                    }
                   
                })
                .catch((error) => {
                }) 
        }
    },
    computed: {
        dataList() {
            let dataSlots = [
                {
                    flex: 1,
                    values: this.slotsArry,
                    className: 'slot1',
                    textAlign: 'center'
                }
            ]
            return dataSlots
        }
    },
    mounted () {
        // this.getUserInfo()
        this.getParams()
    },
    created () {
        
        let isQuality = this.$route.query.isQuality
        console.log(isQuality)
        if (isQuality === 'yes') {
            this.isQuality = true
            this.title = '取消质控委员'
        } else {
            this.isQuality = false
            this.title = '申请质控委员'
        }
        console.log(this.isQuality)
        this.getstate()
        this.maxReplyLength = parseInt(window.sessionStorage.getItem('maxReplyLength'))
        this.remark = this.maxReplyLength
        
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.apply-contol {
    .qualityAgree {
        background: #fff;
        padding: 0 10px 10px;
        p {
            font-size: $font-l;
            color: $color-afont;
            line-height: 24px;
        }
        .dotP {
            font-size: $font-m;
            color: $color-bfont;
            padding: 15px 0 2px;
        }
        .btnCase {padding: 20px 10px;}
    }
    
    // .hintTop {background: #fff;padding: 10px;}
    // .apply-row {padding-bottom: 10px; padding-top: 10px; position: relative; background: $color-wfont; margin-top: 10px;}
    // .apply-row > p { padding: 0 0 5px 10px;}
    // .stateShow {background-color: #fff; padding: 10px;}
    // .apply-row .hint {font-size: $font-xl;}
    // .apply-row .cancel-btn {padding: 10px;}
    // .apply-row .green-hint {text-align: center; font-size: 22px; line-height: 50px;}
    // .mint-radiolist .mint-cell-title {display: block;}
    // .apply-row .mint-cell-title {display: none;}
    // .font-number {position: absolute; bottom: 10px; right: 20px; color: color-afont;}
    // .okCase {padding: 20px 10px;}
    // .apply-title {
    //     text-align: center;
    //     font-size: 20px;
    //     line-height: 40px;
    // }
    // .illness-label::after {content: '*'; color: $color-red;vertical-align: bottom; line-height: 18px; padding-left: 1px;}
    // .illness-case {
    //     padding: 5px 10px;
    //     display: flex;
    //     .illness-label {
    //         width: 105px;
        
    //     }
    //     .illness-value {
    //         flex: 1;
    //     }
    // }
}

</style>