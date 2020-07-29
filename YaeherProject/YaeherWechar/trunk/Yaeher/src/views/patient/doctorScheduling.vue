<template>
    <div class="doctorScheduling padding-top">
        <mt-header fixed title="门诊排班">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">            
            <ul class="server-margin1">
                <li v-if="item.serviceState" v-for="(item, index) in doctorScheduling" :key="index" class="flex common-flex">
                    <div class="common-left">
                        <p class="blod">{{item.duplication}}  &nbsp; {{item.strArry}}</p>
                        <p>{{item.clinicIDAdd}}</p>
                        <p>{{item.clinicType}} 挂号费：{{item.registrationFee}}元</p>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
			id: 0,
			doctorScheduling: []
        }
	},
	methods: {
		collectDoctor() {
			this.collectDoctor()
		},
		getDoctorDetail() { // 获取医生详情
            const id = this.id
            this.instance.clinicDoctor({
                id
            })
                .then((response) => {
                    const doctorDetail = response.data.result.item
                    this.doctorScheduling = doctorDetail.doctorScheduling
                    for (let i = 0; i < this.doctorScheduling.length; i++) {
                        let schedulingTime = JSON.parse(this.doctorScheduling[i].schedulingTime)
                        let clinicType = JSON.parse(this.doctorScheduling[i].clinicType).Value
                        let duplication = JSON.parse(this.doctorScheduling[i].duplication).Value
                        this.doctorScheduling[i].clinicType = clinicType
                        this.doctorScheduling[i].duplication = duplication
                        let timeArry = new Array()
                        for (let j = 0; j < schedulingTime.length; j++) {
                            timeArry.push(schedulingTime[j].Value)
                        }
                        let strArry = timeArry.join(',')
                        this.doctorScheduling[i].strArry = strArry
                    }
                    console.log(this.doctorScheduling)
                })
                .catch((error) => {
                    console.log('error')
                }) 
		}
	},
    mounted () {
        this.getDoctorDetail()
    },
    created () {
        this.id = parseInt(this.$route.query.id)
        window.sessionStorage.setItem('doctorId', this.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctorScheduling {
    .server-margin1 {
        margin: 10px;
        padding: 0 0 0 15px;
        .common-flex {
            .common-left {
                display: block;
                p {
                    font-size: $font-m;
                    color: $color-bfont;
                    line-height: 20px;
                }
                .blod {
                    font-size: $font-l;
                    color: $color-afont;
                    line-height: 22px;
                }
            }
        }
    }
}

</style>