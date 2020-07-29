<template>
    <div class="evaluate padding-top">
		<mt-header fixed title="评价">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="evaCase">
				<div style="text-align: center;" class="evaAvatar">
					<router-link :to="{path: '/doctor-detail-patient', query: {id: doctorId}}"><img style="width: 40px; height: 40px; border-radius: 5px;" :src="doctorAvatar" /></router-link>
					<p>{{doctorName}}</p>
				</div>
				<div v-if="0 === giveStar">
					<div class="star-title">
						<div class="font-o font-small">
							<div class="give-star">
								<div @click="playStar(index)" v-for="(value,index) in 5" :key="index" :class="{starOk: value <= giveStar, starNo: value > giveStar}"></div>
							</div>
						</div>
					</div>
				</div>
				<div v-if="item.labelCode === giveStar" v-for="(item,index1) in evaluateDetail" :key="index1">
					<div class="star-title">
						<div class="font-o font-small">
							<div class="give-star">
								<div @click="playStar(index)" v-for="(value,index) in 5" :key="index" :class="{starOk: value <= giveStar, starNo: value > giveStar}"></div>
							</div>
						</div>
						
						<p class="font-o">{{item.labelName}}</p>
					</div>
					<div class="label-case">
						<span v-for="(item1, index2) in item.children" @click="selectLabel(index2)" :key="index2" class="label-o" :class="{checkedO: item1.checked}">{{item1.labelName}}</span>
					</div>
				</div>
				
				<div class="mui-input-row evaluate-row">
                    <p>评价描述：</p>
                    <mt-field label="评价" @input = "descInput" placeholder="填写评价信息" :attr="{ maxlength: maxReplyLength }" type="textarea" rows="5" v-model="evaluateContent"></mt-field>
					<span class="font-number">可输入{{remark}}字</span>
				</div>
                <div class="okCase">
                    <div @click="submitEvaluate" class="okBtn">
                        <mt-button type="primary" class="mint-button--large">提交评价</mt-button>
                    </div>
                </div>
			</div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import { fontVery } from 'assets/js/common.js'
export default {
    data () {
        return {
            introduction: '',
            show: false,
			evaluateDetail: [],
			idArray: [],
			giveStar: 0,
			maxReplyLength: 500,
			clickFlag: true,
			remark: 500,
			evaluateLevel: 0,
			doctorId: 0,
			evaluateContent: '',
			doctorName: '',
			doctorAvatar: '',
			labelId: 0,
        }
    },
	methods: {
		descInput() {
            let txtVal = this.evaluateContent.length;
            this.remark = this.maxReplyLength - txtVal;
        },
		playStar (index) { // 打分
			this.labelId = this.evaluateDetail[index].id
			console.log(this.labelId)
			this.giveStar = index + 1
			console.log(this.evaluateDetail)
			
        },
        selectLabel (index) {
			let giveStar = this.giveStar - 1
			console.log(this.evaluateDetail[giveStar].children)
			this.evaluateDetail[giveStar].children[index].checked = !this.evaluateDetail[giveStar].children[index].checked
			var strArry = new Array()
			for (let i = 0; i < this.evaluateDetail[giveStar].children.length; i++) {
				if (this.evaluateDetail[giveStar].children[i].checked) {
					let obj = {
						id: this.evaluateDetail[giveStar].children[i].id
					}
					strArry.push(obj)
				}
			}
			this.idArray = strArry
			console.log(this.idArray)
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
		submitEvaluate () { // 提交评价
			
			let giveStar = this.giveStar
			const consultID = this.id
			const labelId = this.labelId
			const evaluateContent = this.evaluateContent
			const titleList = this.idArray
			console.log(giveStar)
			if(labelId == 0) {
				Toast('评分不能为空')
				this.clickFlag = true
				return
			}
			if (giveStar <= 3) {
				if (!evaluateContent) {
					Toast('评价描述不能为空')
					this.clickFlag = true
					return
				}
			}
			// let fontFlag = fontVery(evaluateContent)
			// if (!fontFlag) return
			if(!this.clickFlag) {
				Toast('请勿重复提交')
				return
			}
			this.clickFlag = false
            this.instance.createConsultationEvaluation({
				consultID,
				labelId,
				evaluateContent,
				titleList,
            })
                .then((response) => {
					if (response.data.result.code === 200) {
						Toast('评价成功')
						this.clickFlag = true
						this.$router.push({ path: '/user-record'})
					}
					
                })
                .catch((error) => {
					this.clickFlag = true
                }) 
		},
		getChargebackList () {
			let labelTypeCode = 'StarClass'
			let _this = this
            this.instance.yaeherLabelListByCode({
                labelTypeCode
            })
                .then((response) => {
					this.evaluateDetail = response.data.result.item
					// this.labelId = this.evaluateDetail[this.giveStar-1].id
					for (let i=0; i<this.evaluateDetail.length;i++) {
						this.evaluateDetail[i].labelCode = parseInt(this.evaluateDetail[i].labelCode)
						for (let j=0; j<this.evaluateDetail[i].children.length;j++) {
							_this.$set(this.evaluateDetail[i].children[j], 'checked', false)
						}
					}
					console.log(this.evaluateDetail)
                })
                .catch((error) => {
                }) 
		}
	},
	mounted () {
		this.getChargebackList()
		this.replyParameter()
	},
	created () {
		this.id = parseInt(this.$route.query.id)
		this.doctorId = window.sessionStorage.getItem('doctorId')
		this.doctorName = window.sessionStorage.getItem('doctorName')
		this.doctorAvatar = window.sessionStorage.getItem('doctorAvatar')
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.evaluate {
	.give-star .starOk{width: 20px; height: 20px;}
	.give-star .starNo{width: 20px; height: 20px;}
	.give-star .starBan{width: 20px; height: 20px;}
    .evaCase {background: $color-wfont; padding: 20px 15px 50px;}
    .evaluate-row {padding-bottom: 20px; padding-top: 20px; position: relative;}
    .evaluate-row > p { padding: 0 0 5px 10px;}
    .label-case {width: 300px; margin: 0 auto;}
    .label-o {color: $color-bfont; display: inline-block; padding: 3px 5px; height: 14px; font-size: $font-m; line-height: 15px; border-radius: 9px; border: 1px solid $color-bfont; margin: 5px;}
    .checkedO {color: $color-or; border: 1px solid $color-or;}
    .label-o input {display: none;}
    .mint-cell-title {display: none;}
    .okCase {padding: 0 0px 20px 0px; background: $color-wfont;}
    .font-number {position: absolute; bottom: 10px; right: 20px; color: $color-bfont;}
    .star-title {text-align: center; padding: 15px 0;}
    .star-title .font-small {padding-bottom: 5px;}
}

</style>