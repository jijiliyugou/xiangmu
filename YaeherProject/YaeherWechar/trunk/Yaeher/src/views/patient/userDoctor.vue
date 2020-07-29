<template>
    <div class="doctor-list-patient padding-top">
        <mt-header fixed title="我的医生">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <search-case :searchType=searchType></search-case>
            <ul class="group1-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <router-link :to="{path: '/doctor-detail-patient', query: {id: item.id}}" class="doctor-a">
                        <div class="flex doctorInfo">
                            <img :src="item.userImage" align="avater" />
                            <div class="flexCase">
                                <p class="doctor-name">
                                    {{item.doctorName}} <i>{{item.title}}</i>
                                    <span v-if="item.serviceState&&item.receiptState"></span>
                                    <span v-if=" !item.receiptState && item.serviceState">已满额</span>
                                    <span v-if="!item.serviceState">休息中</span>
                                </p>
                                <p class="star-font"><star :star=item.averageEvaluate></star><span v-if="item.averageEvaluate!=0">{{item.averageEvaluate}}</span></p>
                                <p>{{item.hospitalName}}</p>
                                <!-- <p>{{item.title}}</p> -->
                                <p class="doctorDis"><span v-for="(item1, index1) in item.doctorslable" :key="index1">
                                    {{item1.lableName}}
                                    </span></p>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>      
        </div>
    </div>
</template>

<script>
import Star from 'components/star/star'
import searchCase from 'components/search/searchCase'
export default {
    components: {
      Star,
      searchCase
    },
    data () {
        return {
            searchValue: '',
            doctorState: 1,
            id: 1,
            skipCount: 1,
            maxResultCount: 10,
            doctorList: [],
            searchType: 'user'
        }
    },
    methods: {
        getYaeherClinicDoctors() { // 获取医生列表
            this.instance.yaeherPatientDoctorPage({
                maxResultCount: 100
            })
                .then((response) => {
                    this.doctorList = response.data.result.item.items
                    const statusArry = ['可接单', '可接单', '今日已满额', '休息中']
                    for(let i = 0; i < this.doctorList.length ;i++ ) {
                        const statusNub = parseInt(this.doctorList[i].status)
                        this.doctorList[i].status = statusArry[statusNub]
                    }
                })
                .catch((error) => {
                    console.log('专业分组请求失败')
                }) 
        }
    },
    mounted () {
        this.getYaeherClinicDoctors()
    },
    created () {
        this.id = window.sessionStorage.getItem('userId')
    },
    activated () {
        this.id = window.sessionStorage.getItem('userId')
        this.getYaeherClinicDoctors()
    }
}
</script>


<style lang="scss">
@import "~assets/sass/base.scss";
 
.doctor-list-patient {
    .give-star .starNo {
        width: 12px;
        height: 12px;
    }
    .give-star .starOk {
        width: 12px;
        height: 12px;
    }
    .give-star .starBan {
        width: 12px;
        height: 12px;
    }
    .group1-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            display: flex;
            .doctor-a {
                display: block;
                width: 100%;
            }
        }
    }
    .doctorInfo {
        img{
            display: block;
            width: 60px;
            height: 60px;
            border-radius: 5px;
        }
    }  
    .doctorDis {
        height: 25px;
        overflow: hidden;
        span{
            display: inline-block; 
            margin-right: 10px; 
            margin-bottom: 3px;
            padding: 5px 4px; 
            font-size: 12px; 
            color: $color-bfont; 
            line-height: 14px;
            height: 12px; 
            border-radius: 10px; 
            border: 1px solid #ddd;
        }
    }
    .flexCase {
        padding: 0 10px; 
    }
    .flexCase p {
        font-size: $font-m;
        color: $color-bfont;
        padding-bottom: 2px;
    }
    .flexCase .doctor-name {
        font-size: $font-xl;
        color: $color-afont;
        span {
            color: $color-star;
            font-size: $font-m;
            float: right;
        }
        i {
            display: inline-block;
            padding-left: 5px;
            font-style: normal;
            font-size: $font-m;
            color: #666;
        }
    }
    .star-font {color: $color-star;}
} 

</style>