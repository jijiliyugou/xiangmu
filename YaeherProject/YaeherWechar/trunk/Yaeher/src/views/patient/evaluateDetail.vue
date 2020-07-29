<template>
    <div class="evaluate-detail padding-top">
		<mt-header fixed title="评价详情">
            <a v-if="rShow != 'no'" @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="evaCase">
				<div style="text-align: center;" class="evaAvatar">
					<router-link v-if="typeEvaluate!=3" :to="{path: '/doctor-detail-patient', query: {id: evaluateDetail.doctorID}}">
						<img :src="evaluateDetail.doctorImage" />
					</router-link>
					<router-link v-if="typeEvaluate===3" :to="{path: '/look-detail', query: {id: evaluateDetail.doctorID}}">
						<img :src="evaluateDetail.doctorImage" />
					</router-link>
					<p>{{evaluateDetail.doctorName}}</p>
				</div>
				<div class="star-title">
					<p class="font-o font-small">
						<star :star=evaluateDetail.evaluateLevel></star>
					</p>
					<p class="font-o">{{reason.LabelName}}</p>
				</div>
				<div class="label-case">
					<span v-for="(item, index) in evaluateReason" :key="index" class="label-o">{{item.LabelName}}</span>
				</div>
                <p class="patient-evaluate">{{evaluateDetail.evaluateContent}}</p>
                <div v-if="evaluateDetail.isQuality" class="star-title">
                    <p>质控评分</p>
					<p class="font-o font-small">
						<star :star=evaluateDetail.qualityLevel></star>
					</p>
					<p class="font-o">{{evaluateDetail.qualityReason}}</p>
				</div>
                <div v-if="typeEvaluate===1" class="okCase">
                    <router-link :to="{path: '/record-detail', query: {id: evaluateDetail.consultID}}" consultID class="okBtn">
                        <mt-button type="primary" class="mint-button--large">查看咨询详情</mt-button>
                    </router-link>
                </div>
				<div v-if="typeEvaluate===3" class="okCase">
                    <router-link :to="{path: '/order-detail', query: {id: evaluateDetail.consultID}}" class="okBtn">
                        <mt-button type="primary" class="mint-button--large">查看咨询详情</mt-button>
                    </router-link>
                </div>
				<div v-if="typeEvaluate===2" class="okCase">
                    <router-link :to="{path: '/order-control', query: {id: evaluateDetail.consultID, admin}}" class="okBtn">
                        <mt-button type="primary" class="mint-button--large">查看咨询详情</mt-button>
                    </router-link>
                </div>
			</div>
        </div>
    </div>
</template>

<script>
import Star from 'components/star/star'
import { createSecret } from 'assets/js/common.js'
export default {
	components: {
      Star
    },
    data () {
        return {
			id: 0,
            introduction: '',
			show: false,
			rShow: '',
			admin: '',
			typeEvaluate: 1,
			consultNumber: '',
			evaluateDetail: {},
			evaluateReason: [],
			reason: {}
        }
	},
	methods: {
		getevaluateDetail () { //获取评价详情
			const id = this.id
			const consultNumber = this.consultNumber
			let params = {}
			if (id === 0 ) {
				params = {
					consultNumber
				}
			} else {
				params = {
					id
				}
			}
			if (this.typeEvaluate != 1) {
				
				this.instance.consultationEvaluationByIdC(
					params
				)
					.then((response) => {
						this.evaluateDetail = response.data.result.item
						this.evaluateReason = JSON.parse(this.evaluateDetail.evaluateReason)
						let reason = this.evaluateDetail.reason
						this.reason = JSON.parse(reason)
						this.reason.LabelCode = parseInt(this.reason.LabelCode)
					})
					.catch((error) => {
					}) 
			} else {
				this.instance.consultationEvaluationById({
					id,
				})
					.then((response) => {
						this.evaluateDetail = response.data.result.item
						this.evaluateReason = JSON.parse(this.evaluateDetail.evaluateReason)
						let reason = this.evaluateDetail.reason
						this.reason = JSON.parse(reason)
						this.reason.LabelCode = parseInt(this.reason.LabelCode)
					})
					.catch((error) => {
					}) 
			}            
		}
	},
	mounted () {
		this.getevaluateDetail()
	},
	created () {
		// this.id = parseInt(this.$route.query.id)
		let id = this.$route.query.id
		this.rShow = this.$route.query.rShow
		if (id) {
			this.id = parseInt(id)
		} else {
			this.consultNumber = this.$route.query.consultNumber
			// this.consultNumber = window.sessionStorage.getItem('consultNumber')
			this.returnShow = false
		}

		this.admin = this.$route.query.admin
		let typeEvaluate1 = this.$route.query.typeEvaluate
		if (typeEvaluate1) { // 判断类型typeEvaluate，1患者端评价，2质控端评价，3医生端评价,
			this.typeEvaluate = parseInt(typeEvaluate1)
		} else {
			this.typeEvaluate = 1
		}
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.evaluate-detail {
	.evaCase {background: $color-wfont; padding: 20px 15px 50px;}
	.evaAvatar {
		img {
			width: 40px; height: 40px; border-radius: 5px;
		}
	}
	.evaluate-row {padding-bottom: 20px; padding-top: 20px; position: relative;}
	.evaluate-row > p { padding: 0 0 5px 10px;}
	.label-case {width: 300px; margin: 0 auto;}
	.label-o {color: $color-or; display: inline-block; padding: 3px 5px; height: 14px; font-size: $font-m; line-height: 14px; border-radius: 9px; border: 1px solid #EC971F; margin: 5px;}
	.mint-cell-title {display: none;}
	.okCase {padding: 0 0px 20px 0px; background: $color-wfont;}
	.font-number {position: absolute; bottom: 10px; right: 20px; color: $color-wfont;;}
	.star-title {text-align: center; padding: 15px 0;}
	.star-title .font-small {padding-bottom: 5px;}
	.patient-evaluate {padding: 20px 30px; font-size: $font-l; color: $color-bfont; text-align: center;}
}

</style>