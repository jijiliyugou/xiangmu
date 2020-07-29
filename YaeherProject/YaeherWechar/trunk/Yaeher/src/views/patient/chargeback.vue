<template>
    <div class="chargeback padding-top">
        <mt-header fixed title="退单">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <mt-radio
                v-if="chargeType!='doctor'"
                title="退单原因"
                v-model="labelId"
                :options="options">
            </mt-radio>
            <div v-if="chargeType==='doctor'">
                <!-- <mt-field label="推荐医生" placeholder="请输入推荐医生名" v-model="recommendDoctorName"></mt-field> -->
                <div class="searchCase">
                    <div class="searchBox">
                        <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="搜索推荐医生，名字查询" />
                        <div @click="searchDoctor">搜索</div>
                    </div>
                </div>
                
                <ul v-if="doctorList.length > 0" class="doctorList">
                    <li v-for="(item, index) in doctorList" :key="index" @click='selectDoctor(item.id, item.userImage, item.doctorName)'>
                        <div class="imgCase">
                            <img :src="item.userImage" align="avater" />
                        </div>
                        <div class="nameCase">
                            {{item.doctorName}}
                            <span class="red-hint">推荐该医生</span>
                        </div>
                    </li>
                </ul>
                <div class="applyDoctor">
                    <!-- 搜索推荐医生 -->
                    <div class="applyLabel">推荐医生：</div>
                    <div v-if="show" class="applyInfo">
                        可搜索推荐医生
                    </div>
                    <div v-if="!show" class="applyInfo">
                        <div class="imgCase">
                            <img :src="doctorUrl" align="avater" />
                        </div>
                        <div class="nameCase">
                            {{doctorName}}
                        </div>
                    </div>
                </div>
            </div>
            <div class="mui-input-row evaluate-row">
                <p>退单原因描述：</p>
                <mt-field label="评价"  @input = "descInput" placeholder="填写退单原因描述" :attr="{ maxlength: maxReplyLength }" type="textarea" rows="5" v-model="refundRemarks"></mt-field>
                <span class="font-number">可输入{{remark}}字</span>
            </div>
            <div class="okCase">
                <div  @click="submitChargeback" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">提交</mt-button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { fontVery } from 'assets/js/common.js'
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            keyWord: '',
            doctorID: 0,
            doctorUrl: '',
            doctorName: '',
            doctorList: [],
            show: true,
            labelId: '0',
            refundRemarks: '',
            maxReplyLength: 500,
            remark: 500,
            chargebackList: [],
            options: [],
            chargeType: '',
            recommendDoctorName: '',
            clickFlag: true
        }
    },
	methods: {
        descInput() {
            let txtVal = this.refundRemarks.length;
            this.remark = this.maxReplyLength - txtVal;
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchDoctor()
            } 
        },
        selectDoctor (id, url, name) {
            this.doctorID = id
            this.doctorUrl = url
            this.doctorName = name
            this.show = false
        },
        searchDoctor() { // 搜索医生
            const keyWord = this.keyWord
            if (!keyWord) {
                Toast('请输入搜索内容')
                return
            }
            this.$refs.inputText.blur()
            this.instance.doctorSearch({
                keyWord,
                onlineState: ''
            })
                .then((response) => {
                    this.doctorList = response.data.result.item.items
                    if (this.doctorList.length === 0) return Toast('没有搜索结果')
                    console.log(this.doctorList)
                })
                .catch((error) => {
                    console.log('科室对应标签')
                }) 
            
        },
        getChargebackList (labelTypeCode1) {
            let labelTypeCode = labelTypeCode1 
            this.instance.yaeherLabelListByCode({
                labelTypeCode
            })
                .then((response) => {
                    this.chargebackList = response.data.result.item[0].children
                    this.labelId = `${this.chargebackList[0].id}`
                    console.log(this.labelId)
                    for(let i = 0 ; i < this.chargebackList.length; i++) {
                        let optionObj = {
                            label: this.chargebackList[i].labelName,
                            value: `${this.chargebackList[i].id}`
                        }
                        this.options.push(optionObj)
                        console.log(this.options)
                    }
                })
                .catch((error) => {
                }) 
        },
        replyParameter () { // 提交追问参数
            this.instance.consultationReplyParameter({
            })
                .then((response) => {
					let replyList = response.data.result.item
					this.maxReplyLength = replyList.maxReplyLength
					this.remark = replyList.maxReplyLength
                })
                .catch((error) => {
                }) 
		},
        submitChargeback () { // 提交退单
            if(!this.clickFlag) {
				Toast('请勿重复提交')
				return
            }
            const chargeType = this.chargeType
            const consultID = this.id
            const recommendDoctorID = this.doctorID
            const refundRemarks = this.refundRemarks
            let labelId = parseInt(this.labelId)
            if (chargeType === 'doctor') {
                labelId = 0
                if (!refundRemarks) {
                    Toast('退单原因描述必填')
                    return
                }
            }
            // let fontFlag = fontVery(refundRemarks)
            // if (!fontFlag) return
            this.$indicator.open({
                text: '退单中，请稍候',
                spinnerType: 'fading-circle'
            })
            this.clickFlag = false
            let params = {
                consultID,
                labelId,
                refundRemarks
            }
            if (recommendDoctorID != 0) {
                params.recommendDoctorID = recommendDoctorID
            }
            if(chargeType) {
                this.instance.createRefundManageD(
                    params
                )
                .then((response) => {
                    if(response.data.result.code === 200) {
                        this.$indicator.close()
                        Toast('退单成功')
                        if (chargeType === 'doctor') {
                            this.$router.push({ path: '/index-doctor'})
                        } if (chargeType === 'control') {
                            this.$router.go(-1)
                        }
                    } else {
                        this.$indicator.close()
                        Toast('退单失败')
                    }
                })
                .catch((error) => {
                    this.$indicator.close()
                    this.clickFlag = true
                }) 
            } else {
                this.instance.createRefundManage(
                    params
                )
                .then((response) => {
                    if(response.data.result.code === 200) {
                        this.$indicator.close()
                        Toast('退单成功')
                        this.$router.push({ path: '/user-record'})
                    } else {
                        this.$indicator.close()
                        Toast('退单失败')
                    }
                })
                .catch((error) => {
                    this.$indicator.close()
                    this.clickFlag = true
                }) 
            }
            
            
        }
	},
	mounted () {
        if (this.chargeType === 'control') {
            this.getChargebackList('QualityControlReturnLabel')
        } else {
            this.getChargebackList('PatientReturnLabel')
        }
        this.replyParameter()
	},
	created () {
        this.id = parseInt(this.$route.query.id)
        this.chargeType = this.$route.query.chargeType
        console.log(this.chargeType)
        
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.chargeback {
    .searchCase {
        padding: 5px;
    }
    .mint-radiolist {background: $color-wfont;}
    .mint-radiolist .mint-radiolist-title {margin: 0; padding: 10px 0 10px 10px;}
    .evaluate-row {padding-bottom: 20px; padding-top: 20px; position: relative; background: $color-wfont;}
    .evaluate-row > p { padding: 0 0 5px 10px;}
    .mint-radiolist .mint-cell-title {display: block;}
    .evaluate-row .mint-cell-title {display: none;}
    .font-number {position: absolute; bottom: 10px; right: 20px; color: color-afont;}
    .okCase {padding: 20px 15px;}
    .applyDoctor {
        background-color: #fff;
        padding: 10px;
        display: flex;
        line-height: 40px;
        .applyLabel {
            width: 100px;
        }
        .applyInfo {
            flex: 1;
            display: flex;
            .imgCase {
                width: 40px;
                height: 40px;
                img {
                    width: 100%;
                    height: 100%;
                    border-radius: 5px;
                }
            }
            .nameCase {
                flex: 1;
                padding-left: 10px;
            }
        }
    }
    .doctorList {
        height: 142px;
        overflow: auto;
        li {
            display: flex;
            .imgCase {
                width: 40px;
                height: 40px;
                img {
                    width: 100%;
                    height: 100%;
                    border-radius: 5px;
                }
            }
            .nameCase {
                flex: 1;
                line-height: 40px;
                padding-left: 10px;
                font-size: $font-l;
                span {
                    float: right;
                }
            }
        }
    }
}

</style>