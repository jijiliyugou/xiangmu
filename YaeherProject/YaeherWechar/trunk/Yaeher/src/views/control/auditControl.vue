<template>
    <div class="audit-control padding-top">
        <mt-header fixed title="质控委员申请">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生名查找" />
                    <div @click="getYaeherClinicDoctors(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getYaeherClinicDoctors"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="group2-list">
                <router-link v-for="(item, index) in doctorList" :key="index" tag="li" :to="{path: '/audit-apply', query: {id: item.qualityCommitteeRegisterId}}">
                    <div class="flex doctorInfo">
                        <router-link :to="{path: '/doctor-detail-patient', query: {id: item.id}}" class="img-case">
                            <img :src="item.userImage" />
                        </router-link>
                        <div class="flexCase">
                            <p class="doctor-name">
                                {{item.doctorName}}
                            </p>
                            <p class="star-font"><star :star=item.averageEvaluate></star> {{item.averageEvaluate}}</p>
                            <p><span>{{item.department}}</span></p>
                            <p><span>{{item.hospitalName}}</span> <span>{{item.title}}</span></p>
                            <div v-if="item.checkState === 'checking'" class="operation-control red-hint">
                                待处理
                            </div>
                            <div v-if="item.checkState === 'fail'"  class="operation-control red-hint">
                                不通过
                            </div>
                            <div v-if="item.checkState === 'success'"  class="operation-control green-hint">
                                已通过
                            </div>
                        </div>
                    </div>
                </router-link>
            </ul>        
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
import Star from 'components/star/star'
export default {
    components: {
      Star
    },
    data () {
        return {
            keyWord: '',
            doctorList: [],
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true
        }
    },
    methods: {
        getYaeherClinicDoctors(flag) { // 获取医生列表
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const keyWord = this.keyWord
            const maxResultCount  = this.maxResultCount 
            console.log(maxResultCount)
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur()
            this.instance.qualityCommitteeRegisterPageC({
                keyWord,
                skipCount,
                maxResultCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        if (flag) this.doctorList = []
                        this.doctorList = this.doctorList.concat(response.data.result.item.items)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                    console.log('获取质控委员申请列表失败')
                }) 
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.getYaeherClinicDoctors(true)
            } 
        }
        
    },
    mounted () {
        this.getYaeherClinicDoctors()
    },
    created () {
    }
}
</script>

<style lang="scss">

@import "~assets/sass/base.scss";
.audit-control {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .group2-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            display: flex;
            .doctorInfo {
                width: 100%;
                .img-case {
                    width: 60px;
                    height: 60px;
                    display: block;
                    img{
                        display: block;
                        width: 60px;
                        height: 60px;
                        border-radius: 5px;
                    }
                }
                .flexCase {
                    padding: 0 10px; 
                    position: relative;
                    flex: 1;
                    p {
                        font-size: $font-m;
                        color: $color-bfont;
                        padding-bottom: 2px;
                    }
                    .doctor-name {
                        font-size: $font-xl;
                        color: $color-afont;
                    }
                    .star-font {
                        color: $color-star;
                    }
                    .operation-control {
                        position: absolute;
                        right: 0;
                        top: 20px;
                        .mint-button {
                            height: 30px;
                            font-size: $font-l;
                            margin-left: 10px;
                        }
                    }
                }
            } 
        }
    }
}


</style>